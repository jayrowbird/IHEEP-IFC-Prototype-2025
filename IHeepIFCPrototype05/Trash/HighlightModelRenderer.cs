// Ignore Spelling: Vertices GPU

using HeepWare.OBJ.Mesh.Data;
using MyOpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace ViewSeparation_PickingWithOrbitCamera
{
    /// <summary>
    /// 
    /// </summary>
    public class HighLightModelRenderer
    {
        private readonly bool DEBUGLOG = false;
        private int highLightModelVertexArrayId = -1;
        private int highLightVertexBufferId = -1;
        private List<VertexPosColorId> gpuVertices = new List<VertexPosColorId>();
        private int highLightShaderId = -1;
        private List<uint> gpuIndexes = new List<uint>();
        private int highLightindexBufferId = -1;
        private Color4 highlightColor = new Color4(250, 0, 0, 0.55f);

        public int GetVertexArrayId()
        {
            return highLightModelVertexArrayId;
        }

        public int GetVertexBufferId()
        {
            return highLightVertexBufferId;
        }

        public int GetGPUIndexBufferId()
        {
            return highLightindexBufferId;
        }

        public int GetGPUIndexBufferCount()
        {
            return gpuIndexes.Count;
        }

        public void ClearGPUMemory()
        {
            if (highLightModelVertexArrayId > -1)
            {
                GL.DeleteVertexArray(highLightVertexBufferId); MY.GLCall(); 
                highLightVertexBufferId = 0 - 1;
            }
            if (highLightindexBufferId > -1)
            { 
                GL.DeleteBuffer(highLightVertexBufferId); MY.GLCall();
                highLightVertexBufferId = -1;
            }
            if (highLightindexBufferId > -1)
            {
                GL.DeleteBuffer(highLightindexBufferId); MY.GLCall();
                highLightindexBufferId = -1;
            }
        }

        public HighLightModelRenderer(int shaderId, MeshObject modelObject)
        {
            //Clean up any old remaining data
            ClearGPUMemory();
            highLightShaderId = shaderId;
            MapModelVertices(modelObject);


            // load the shader to be used render models
            GL.UseProgram(highLightShaderId); MY.GLCall();

            //ArrayBuffer
            MY.GLCall(highLightModelVertexArrayId = GL.GenVertexArray()); MY.GLCall();
            GL.BindVertexArray(highLightModelVertexArrayId); MY.GLCall();

            //VertexBuffer
            // the vertex buffer holds the model positions, color and id
            highLightVertexBufferId = GL.GenBuffer(); MY.GLCall();
            GL.BindBuffer(BufferTarget.ArrayBuffer, highLightVertexBufferId); MY.GLCall();
            GL.BufferData(BufferTarget.ArrayBuffer, gpuVertices.Count * ((3 + 4) * sizeof(float) + (1 * sizeof(int))), gpuVertices.ToArray(), BufferUsageHint.StaticDraw); MY.GLCall();
            MY.GLCall();


            //IndexBuffer
            MY.GLCall(highLightindexBufferId = GL.GenBuffer()); MY.GLCall();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, highLightindexBufferId); MY.GLCall();
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

        public void ClearHiHighLightModel()
        {
            ClearGPUMemory();
            highLightShaderId = -1;
        }

        public void MapModelVertices(MeshObject meshObject)
        {
            for (int i = 0; i < meshObject.position.Count; i++)
            {
                gpuVertices.Add(new VertexPosColorId()
                {
                    position = meshObject.position[i],
                    color = highlightColor,
                    id = (int)meshObject.GetId()
                });
            }
            for (int i = 0; i < meshObject.faceIndexes.Count; i++)
            {
                //load the index buffer
                gpuIndexes.Add((uint)meshObject.faceIndexes[i]);
            }
            return;
        }
    }
}
