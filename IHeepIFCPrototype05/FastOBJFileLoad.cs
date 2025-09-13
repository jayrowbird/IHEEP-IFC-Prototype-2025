using FileFormatWavefront;
using FileFormatWavefront.Model;
using HeepWare.IFC.Catalog;
using HeepWare.Mesh.Utilities;
using HeepWare.OBJ.Data;
using MyOpenTK;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace ViewSeparation_PickingWithOrbitCamera
{
    public class VertexPosColorId_
    {
        public Vector3 position;
        public Color4 color;
        public int id;
    }
    public class FastOBJFileLoad
    {
        private IFCCatalog ifcCatalog = IFCCatalog.Instance;
        private string materialFilename = string.Empty;
        private List<VertexPosColorId_> tmpVertexbuffer = new List<VertexPosColorId_>();
        private List<VertexPosColorId> gpuVertexbuffer = new List<VertexPosColorId>();
        private List<uint> gpuIndexBuffer = new List<uint>();

        private List<Vector3> position = new List<Vector3>();
        private List<Vector3> normal = new List<Vector3>();
        private List<Vector2> texturecoord = new List<Vector2>();

        private List<Material>? materials = new List<Material>();
        private Material? material = null;
        private int meshid = -3;
        private string meshname = string.Empty;
        private Color4 meshcolor = MeshUtils.GetRandomColor();

        public List<uint> GetGPUIndexBuffer()
        {
            return gpuIndexBuffer;
        }

        public List<VertexPosColorId> GetGPUVertexBuffer()
        {
            return gpuVertexbuffer;
        }

        public void LoadFile(string filename)
        {
            if (filename == null)
            {
                throw new Exception(string.Format("File not found {0} or not obj file converted from ifc", filename));
            }

            List<string> lines = File.ReadLines(filename).ToList();

            List<OBJMeshStrings> objMeshesAsStrings = new List<OBJMeshStrings>();

            if (lines.Count > 0)
            {
                //Load Material file if one exists
                materials = LoadOBJMaterialFile(lines, filename);

                //OBJLineStatus linestatus = OBJLineStatus.NewModel;
                for (int i = 0; i < lines.Count; i++)
                {
                    lines[i] = lines[i].Trim();

                    if (lines[i].StartsWith("g") == true || lines[i].StartsWith("o") == true)
                    {
                        meshname = lines[i].Replace("g ","").Replace("o ","").Trim();
                        meshid = ifcCatalog.Add(meshname);
                    }
                    else if (lines[i].StartsWith("mtllib ") == true)
                    {
                        materialFilename = lines[i].Trim().Replace("mtlib ", "");
                    }
                    else if (lines[i].StartsWith("v ") == true)
                    {
                        Vector3? v = MakeVector3(lines[i]);
                        if (v != null) { position.Add((Vector3)v); tmpVertexbuffer.Add(new VertexPosColorId_() { position = (Vector3)v }); }
                        ;
                    }
                    else if (lines[i].StartsWith("vn") == true)
                    {
                        Vector3? v = MakeVector3(lines[i]);
                        if (v != null) { normal.Add((Vector3)v); }
                    }
                    else if (lines[i].StartsWith("vt") == true)
                    {
                        Vector2? v = MakeVector2(lines[i]);
                        if (v != null) { texturecoord.Add((Vector2)v); }
                    }
                    else if (lines[i].StartsWith("use") == true)
                    {
                        string mat = lines[i].Trim();
                        mat = mat.Replace("usemtl", "").Trim();
                        if (materials != null)
                        {
                            material = materials.Where(x => x.Name == mat).SingleOrDefault();
                            if (material != null)
                            {
                                meshcolor = material.Diffuse;
                                meshcolor.A = 0.75f;
                            }
                            else
                            {
                                meshcolor = MeshUtils.GetRandomColor();
                            }
                        }
                        else
                        {
                            meshcolor = MeshUtils.GetRandomColor();
                        }
                    }
                    else if (lines[i].StartsWith("f") == true)
                    {
                        string line = lines[i].Replace("//", ",").Replace("/", ",");
                        string[] items = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        List<int> indexes = new List<int>();

                        //position only
                        for (int p = 1; p < items.Length; p++)
                        {
                            string[] itms = items[p].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            indexes.Add(int.Parse(itms[0]) - 1);
                        }
                        List<uint> pindexes = AddIndexes(indexes);
                        for (int n = 0; n < pindexes.Count; n++)
                        {
                            int p = (int)pindexes[n];
                            tmpVertexbuffer[p].id = meshid;

                            tmpVertexbuffer[p].color = meshcolor;
                        }
                        gpuIndexBuffer.AddRange(pindexes);
                    }
                }
            }

            for (int i = 0; i < tmpVertexbuffer.Count; i++) 
            {
                gpuVertexbuffer.Add(new VertexPosColorId() { position = tmpVertexbuffer[i].position, color = tmpVertexbuffer[i].color, id = tmpVertexbuffer[i].id });
            }
        }
        //private static int colorIndex = 0;

        //private static Color4 GetRandomColor()
        //{
        //    Color4 color;
        //    switch (colorIndex++)
        //    {
        //        case 0:
        //            color = Color4.White; break;

        //        case 1:
        //            color = Color4.Black; break;
        //        case 2:
        //            color = Color4.BlueViolet; break;
        //        case 3:
        //            color = Color4.LimeGreen; break;
        //        case 4:
        //            color = Color4.Pink; break;
        //        case 5:
        //            color = Color4.Orchid; break;
        //        case 6:
        //            color = Color4.DarkCyan; break;
        //        case 7:
        //            color = Color4.AliceBlue; break;
        //        case 8:
        //            color = Color4.Teal; break;
        //        case 9:
        //            color = Color4.OliveDrab; break;
        //        case 10:
        //            color = Color4.DarkGray; break;
        //        default:
        //            colorIndex = 0;
        //            color = Color4.DarkMagenta; break;
        //    }
        //    color.A = 0.15f;
        //    return color;
        //}
        private List<Material>? LoadOBJMaterialFile(List<string> lines, string filename)
        {
            string? materialPath = null;
            if (lines.Count > 0)
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Trim().StartsWith("mtllib") == true)
                    {
                        string[] items = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (items.Length == 2)
                        {
                            DirectoryInfo dirInfo = new DirectoryInfo(filename);
                            if (dirInfo.Parent != null == true && dirInfo.Parent.Exists)
                            {
                                materialPath = dirInfo.Parent.FullName;
                                materialPath = Path.Combine(materialPath, items[1]);
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            if (materialPath != null)
            {
                var fileLoadResult = FileFormatMtl.Load(materialPath, false);
                return fileLoadResult.Model;
            }
            return null;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fi"></param> 
        private List<uint> AddIndexes(List<int> fi)
        {
            List<uint> findex = new List<uint>();
            int loopcount = 1 + fi.Count - 3;
            int j = 1;
            for (int i = 0; i < loopcount; i++)
            {

                findex.Add((uint)fi[0]);


                findex.Add((uint)fi[j]);


                findex.Add((uint)fi[j + 1]);
                j++;
            }

            return findex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private Vector3? MakeVector3(string line)
        {
            string[] items = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (items.Length == 4)
            {
                return new Vector3(float.Parse(items[1]), float.Parse(items[2]), float.Parse(items[3]));
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private Vector2? MakeVector2(string line)
        {
            string[] items = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (items.Length == 3)
            {
                return new Vector2(float.Parse(items[1]), float.Parse(items[2]));
            }
            return null;
        }
    }
}
