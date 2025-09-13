// Ignore Spelling: miny minz maxx maxz

using OpenTK.Mathematics;
using System.Text;

namespace HeepWare.RTree.Library
{

    public struct Bbox
    {
        private readonly bool DebugFlag = true;
        private static uint count = 0;
        private uint internalid = 0;
        private string guid = string.Empty;
        private float[] min = new float[3];
        private float[] max = new float[3];


        public void SetGuid(string v)
        {
            guid = v;
        }

        public float[] GetMin()
        {
            return min;
        }

        public void SetMin(float x, float y, float z)
        {
            min[0] = x; min[1] = y; min[2] = z;
            return;
        }

        public float GetMinX()
        {
            return min[0];
        }

        public void SetMax(float x, float y, float z)
        {
            max[0] = x; max[1] = y; max[2] = z;
            return;
        }

        public float SetMinX(float x)
        {
            return min[0] = x;
        }

        public float GetMinY()
        {
            return min[1];
        }
        public float SetMinY(float y)
        {
            return min[1] = y;
        }

        public float GetMinZ()
        {
            return min[2];
        }

        public float SetMinZ(float z)
        {
            return min[2] = z;
        }
        public float[] GetMax()
        {
            return max;
        }

        public float GetMaxX()
        {
            return max[0];
        }
        public float SetMaxX(float x)
        {
            return max[0] = x;
        }
        public float GetMaxY()
        {
            return max[1];
        }
        public float SetMaxY(float y)
        {
            return max[1] = y;
        }

        public float GetMaxZ()
        {
            return max[2];
        }

        public float SetMaxZ(float z)
        {
            return max[2] = z;
        }

        public string GetGuid()
        {
            return guid;
        }

        public Bbox(Bbox b)
        {
            internalid = b.internalid;
            guid = b.guid;
            for (int i = 0; i < 3; i++)
            {
                min[i] = b.min[i];
                max[i] = b.max[i];
            }
        }
        //public Bbox()
        //{
        //    internalid = count++;
        //}

        //public Bbox(string id) : guid(id)
        //{
        //    internalid = count++;
        //    if (DebugFlag) Console.WriteLine(" {0} Bbox created", guid);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid_"></param>
        /// <param name="minx"></param>
        /// <param name="miny"></param>
        /// <param name="minz"></param>
        /// <param name="maxx"></param>
        /// <param name="maxy"></param>
        /// <param name="maxz"></param>
        public Bbox(string guid_, float minx, float miny, float minz,
                                  float maxx, float maxy, float maxz)
        {
            guid = guid_;
            internalid = count++;

            min[0] = minx;
            min[1] = miny;
            min[2] = minz;

            max[0] = maxx;
            max[1] = maxy;
            max[2] = maxz;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid_"></param>
        /// <param name="min_"></param>
        /// <param name="max_"></param>
        public Bbox(string guid_, float[] min_, float[] max_)
        {
            guid = guid_;
            internalid = count++;
            for (int i = 0; i < min_.Length; i++)
            {
                min[i] = min_[i];
                max[i] = max_[i];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid_"></param>
        /// <param name="min_"></param>
        /// <param name="max_"></param>
        public Bbox(string guid_, Vector3 min_, Vector3 max_)
        {
            guid = guid_;
            internalid = count++;

            min[0] = min_.X; min[1] = min_.Y; min[2] = min_.Z;
            max[0] = max_.X; max[1] = max_.Y; max[2] = max_.Z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Vector3 Center()
        {
            Vector3 center = new Vector3();
            center.X = (max[0] + min[0]) / 2.0f;
            center.Y = (max[1] + min[1]) / 2.0f;
            center.Z = (max[2] + min[2]) / 2.0f;
            return center;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public void WriteBbox2WaveFrontFile(string filename)
        {

            // Write to the StringBuilder
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("v {0} {1} {2}\n", min[0], min[1], min[2]);
            sb.AppendFormat("v {0} {1} {2}\n", max[0], min[1], min[2]);
            sb.AppendFormat("v {0} {1} {2}\n", max[0], max[1], min[2]);
            sb.AppendFormat("v {0} {1} {2}\n", min[0], max[1], min[2]);
            sb.AppendFormat("v {0} {1} {2}\n", min[0], min[1], max[2]);
            sb.AppendFormat("v {0} {1} {2}\n", max[0], min[1], max[2]);
            sb.AppendFormat("v {0} {1} {2}\n", max[0], max[1], max[2]);
            sb.AppendFormat("v {0} {1} {2}\n", min[0], max[1], max[2]);

            sb.AppendLine("//front");
            sb.AppendLine("f 1 2 3 ");
            sb.AppendLine("f 3 4 1 ");
            sb.AppendLine("//back");
            sb.AppendLine("f 5 6 7 ");
            sb.AppendLine("f 7 8 5 ");
            sb.AppendLine("//bottom");
            sb.AppendLine("f 1 2 6 ");
            sb.AppendLine("f 6 5 1 ");
            sb.AppendLine("//top");
            sb.AppendLine("f 4 3 7 ");
            sb.AppendLine("f 7 8 4 ");
            sb.AppendLine("//left");
            sb.AppendLine("f 1 5 8 ");
            sb.AppendLine("f 8 4 1 ");
            sb.AppendLine("//right");
            sb.AppendLine("f 2 6 7 ");
            sb.AppendLine("f 7 3 2 ");

            // Write to the file
            File.WriteAllText(filename, sb.ToString());
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bbox"></param>
        /// <returns></returns>
        public static List<Triangle> MakeBboxSurface(Bbox bbox)
        {
            List<Triangle> surfaces = new List<Triangle>();

            //if (std::is_same_v<Vector3, Vector3> == true)
            {
                Vector3 p1 = new Vector3(bbox.GetMinX(), bbox.GetMinY(), bbox.GetMinZ());
                Vector3 p2 = new Vector3(bbox.GetMaxX(), bbox.GetMinY(), bbox.GetMinZ());
                Vector3 p3 = new Vector3(bbox.GetMaxX(), bbox.GetMaxY(), bbox.GetMinZ());
                Vector3 p4 = new Vector3(bbox.GetMinX(), bbox.GetMaxY(), bbox.GetMinZ());
                Vector3 p5 = new Vector3(bbox.GetMinX(), bbox.GetMinY(), bbox.GetMaxZ());
                Vector3 p6 = new Vector3(bbox.GetMaxX(), bbox.GetMinY(), bbox.GetMaxZ());
                Vector3 p7 = new Vector3(bbox.GetMaxX(), bbox.GetMaxY(), bbox.GetMaxZ());
                Vector3 p8 = new Vector3(bbox.GetMinX(), bbox.GetMaxY(), bbox.GetMaxZ());

                Triangle front1 = new Triangle(p1, p2, p3);
                Triangle front2 = new Triangle(p3, p4, p1);
                surfaces.Add(front1);
                surfaces.Add(front2);

                Triangle back1 = new Triangle(p5, p6, p7);
                Triangle back2 = new Triangle(p7, p8, p5);
                surfaces.Add(back1);
                surfaces.Add(back2);

                Triangle bottom1 = new Triangle(p1, p2, p6);
                Triangle bottom2 = new Triangle(p6, p5, p1);
                surfaces.Add(bottom1);
                surfaces.Add(bottom2);

                Triangle top1 = new Triangle(p4, p3, p7);
                Triangle top2 = new Triangle(p7, p8, p4);
                surfaces.Add(top1);
                surfaces.Add(top2);

                Triangle left1 = new Triangle(p1, p5, p8);
                Triangle left2 = new Triangle(p8, p4, p1);
                surfaces.Add(left1);
                surfaces.Add(left2);

                Triangle right1 = new Triangle(p2, p6, p7);
                Triangle right2 = new Triangle(p7, p3, p2);
                surfaces.Add(right1);
                surfaces.Add(right2);
            } 
            return surfaces;
        }
    }
}
