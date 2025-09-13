// Ignore Spelling: Vertices GPU

using HeepWare.OBJ.Mesh.Data;
using MyOpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace ViewSeparation_PickingWithOrbitCamera
{
    /// <summary>
    /// 
    /// </summary>
    public class BoundBoxRenderer
    {
        private readonly bool DEBUGLOG = false;
        private int bboxVertexArrayId = -1;
        private int bboxVertexBufferId = -1;
        private List<VertexPosColorId> gpuVertices = new List<VertexPosColorId>();
        private int bboxShaderId = -1;
        private List<uint> gpuIndexes = new List<uint>();
        private int bboxIndexBufferId = -1;
        //private Color4 highlightColor = new Color4(250, 0, 0, 0.55f);

        public int GetVertexArrayId()
        {
            return bboxVertexArrayId;
        }

        public int GetVertexBufferId()
        {
            return bboxVertexBufferId;
        }

        public int GetGPUIndexBufferId()
        {
            return bboxIndexBufferId;
        }

        public int GetGPUIndexBufferCount()
        {
            return gpuIndexes.Count;
        }

        private void ClearGPUMemory()
        {
            if (bboxVertexArrayId > -1)
            {
                GL.DeleteVertexArray(bboxVertexBufferId); MY.GLCall();
                bboxVertexBufferId = 0 - 1;
            }
            if (bboxIndexBufferId > -1)
            {
                GL.DeleteBuffer(bboxVertexBufferId); MY.GLCall();
                bboxVertexBufferId = -1;
            }
            if (bboxIndexBufferId > -1)
            {
                GL.DeleteBuffer(bboxIndexBufferId); MY.GLCall();
                bboxIndexBufferId = -1;
            }
            gpuIndexes.Clear();
            gpuVertices.Clear();
        }

        public BoundBoxRenderer(int shaderId, MeshObject meshObject)
        {
            //Clean up any old remaining data
            ClearGPUMemory();
            bboxShaderId = shaderId;
            HeepWare.OBJ.Data.BBox bbox = meshObject.GetBBox();
            gpuVertices = bbox.GetGPUVertices();
            gpuIndexes = bbox.GetGPUIndexes();

            SetGPUState();
        }

        public BoundBoxRenderer(int shaderId, List<MeshObject> meshObjects)
        {
            //Clean up any old remaining data
            ClearGPUMemory();
            bboxShaderId = shaderId;
            uint currentCount = 0;
            for (int i = 0; i < meshObjects.Count; i++)
            {
                HeepWare.OBJ.Data.BBox bbox = meshObjects[i].GetBBox();
                gpuVertices.AddRange(bbox.GetGPUVertices());

                List<uint> bboxIndexes = bbox.GetGPUIndexes();
                for (int n = 0; n < bboxIndexes.Count; n++)
                {
                    gpuIndexes.Add(bboxIndexes[n]+currentCount);
                }
                currentCount += 8;
            }
            SetGPUState();
        }

        public void ClearHiHighLightModel()
        {
            ClearGPUMemory();
            bboxShaderId = -1;
        }

        private void SetGPUState()
        {
            // load the shader to be used render models
            GL.UseProgram(bboxShaderId); MY.GLCall();

            //ArrayBuffer
            MY.GLCall(bboxVertexArrayId = GL.GenVertexArray()); MY.GLCall();
            GL.BindVertexArray(bboxVertexArrayId); MY.GLCall();

            //VertexBuffer
            // the vertex buffer holds the model positions, color and id
            bboxVertexBufferId = GL.GenBuffer(); MY.GLCall();
            GL.BindBuffer(BufferTarget.ArrayBuffer, bboxVertexBufferId); MY.GLCall();
            GL.BufferData(BufferTarget.ArrayBuffer, gpuVertices.Count * ((3 + 4) * sizeof(float) + (1 * sizeof(int))), gpuVertices.ToArray(), BufferUsageHint.StaticDraw); MY.GLCall();
            MY.GLCall();


            //IndexBuffer
            MY.GLCall(bboxIndexBufferId = GL.GenBuffer()); MY.GLCall();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, bboxIndexBufferId); MY.GLCall();
            GL.BufferData(BufferTarget.ElementArrayBuffer, gpuIndexes.Count * sizeof(uint), gpuIndexes.ToArray(), BufferUsageHint.StaticDraw); MY.GLCall();
            MY.GLCall();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0); MY.GLCall();

            //position
            //byte location of the positions data in the vertex buffer
            GL.EnableVertexAttribArray(0); MY.GLCall();
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, ((3 + 4) * sizeof(float) + (1 * sizeof(int))), 0);
            MY.GLCall();

            //Color
            //byte location of the color data in the vertex buffer
            GL.EnableVertexAttribArray(1); MY.GLCall();
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, ((3 + 4) * sizeof(float) + (1 * sizeof(int))), 3 * sizeof(float));
            MY.GLCall();

            //Model id
            //byte location of the 'id' data in the vertex buffer
            GL.EnableVertexAttribArray(2); MY.GLCall();
            GL.VertexAttribIPointer(2, 1, VertexAttribIntegerType.Int, ((3 + 4) * sizeof(float) + (1 * sizeof(int))), 7 * sizeof(float));
            MY.GLCall();

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0); MY.GLCall();
            GL.BindVertexArray(0); MY.GLCall();
            GL.UseProgram(0); MY.GLCall();

            return;
        } 
    }
}
