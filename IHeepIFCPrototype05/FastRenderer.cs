// Ignore Spelling: Vertices Framebuffer

using HeepWare.IFC.Catalog;
using HeepWare.Mesh.Utilities;
using HeepWare.ModelPicking.Framebuffer;
using HeepWare.OBJ.Mesh.Data;
using HeepWare.Windows.Forms.IFC.Prototype;
using MyOpenTK;
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using Songo.OrbitCamera;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ViewSeparation_PickingWithOrbitCamera;
using WinFormsConsoleReadonly;

namespace HeepWare.Renderer
{
    /// <summary>
    /// 
    /// </summary>
    public class FastRenderer
    {
        bool  DEBUGLOG = false;
        private readonly TextBox LogTextBox;

        private MeshUtils meshUtils;

        internal HighLightModelRenderer? highLightModelRenderer = null;
        internal BoundBoxRenderer? boundBoxRenderer = null;
        private List<MeshObject>? models = null; 

        //below is the mapping of the IFC guid and OpenGL render guid
        IFCCatalog ifcCatalog = IFCCatalog.Instance; 
         
        private List<VertexPosColorId> gpuVertices;
        private List<uint> gpuIndexBuffer;

        private ShaderSeparateFiles? shader;
        private int vertexArrayId;
   
        private int VertexBufferId;
        private int indexBufferId;

        private OrbitCamera? orbitCamera = null;
        private Vector3 m_sceneCenter, m_sceneMin, m_sceneMax;

        private Matrix4 target;
        private Matrix4 projection;
        private Matrix4 mvp;

        private int framebufferId = -1;
        private int shaderID = -1;
        private int screenShaderID = -1;

        private int screenQuadVAO = -1;

        private int textureModelColorbuffer = -1;
        private int textureModelColorbufferUnit = -1;

        private int currentModelId = -2;
        private int selectedModelId = -2;

        private GLControl refGlControl;

        private int screenWidth;
        private int screenHeight;

        //-----------------------  
        //private float _z = -2.7f;
        private float playTime;     // sec
        //-----------------------

        //Moving Frame buffer creation to a class lib
        private Framebuffer4ModelPicking? framebuffer4ModelPicking = null;
        //-------------------------

        private Vector3 cameraPosition;
        private Vector3 targetPosition;

        IheepIFCPrototypeForm refMainWindowForm;
        public FastRenderer(IheepIFCPrototypeForm mainWindowForm_, List<VertexPosColorId> GPUVertices, List<uint> GPUIndexBuffer)
        {
            meshUtils = new MeshUtils();
            // reference to main viewer form so we can up date the tile with fps
            refMainWindowForm = mainWindowForm_;
            //reference to the main viewer forms GLcontrol so we can handle resize events
            refGlControl = refMainWindowForm.glControl1;
            // reference to the log text box panel so we can display error text messages
            LogTextBox = refMainWindowForm.LogTextBox;
            //load vertex and index buffers from the list of mesh objs

            gpuVertices = GPUVertices;
            gpuIndexBuffer = GPUIndexBuffer;

            meshUtils.ComputeBoundingBox2(gpuVertices, ref m_sceneCenter, ref m_sceneMin, ref m_sceneMax);

            // setup camera
            // setup frame buffer for model picking
            // load and compile shaders
            // create vertex buffers and index buffers
            // enable vertex attributes attributes
            // setup mvp matrices 
            RendererSetup();

            InitializeGPUContext(); 
        }

        /// <summary>
        /// 
        /// </summary>
       public void CleanUpGPUMemory()
        {
            framebuffer4ModelPicking.CleanupGPUMemory();

            if (VertexBufferId > 0)
            {
                GL.DeleteBuffer(VertexBufferId); MY.GLCall();
            }

            if (indexBufferId > 0)
            {
                GL.DeleteVertexArray(indexBufferId); MY.GLCall();
            }

            if (vertexArrayId > 0)
            {
                GL.DeleteVertexArray(vertexArrayId); MY.GLCall();
            }
            if (gpuVertices.Count > 0)
            {
                gpuVertices.Clear();
            }
            if (gpuIndexBuffer.Count > 0)
            {
                gpuIndexBuffer.Clear();
            }
            if (models != null && models.Count > 0)
            {
                models.Clear();
            }

            if (vertexArrayId > 0)
            {
                GL.DeleteVertexArray(vertexArrayId); MY.GLCall();
            }

            //Release Screen Quad frame buffer resources
            if (textureModelColorbuffer > 0)
            {
                GL.DeleteTexture(textureModelColorbuffer); MY.GLCall();
            }
            if (textureModelColorbuffer > 0)
            {
                GL.DeleteTexture(textureModelColorbuffer); MY.GLCall();
            }

        }
        /// <summary>
        /// return a reference to the main orbit camera to the main form viewer
        /// this supports mouse camera events
        /// </summary>
        /// <returns></returns>
        public OrbitCamera? GetOrbitCamera()
        {
            return orbitCamera;
        }

        public int GetSelectedModelId()
        {
            return selectedModelId;
        }

        public void SetModels(List<MeshObject> _models)
        {
            models = _models;
        }

        //bool toggle = true;
        public int SelectCurrentModel()
        {
            if (currentModelId != -1 && models != null)
            {
                MeshObject? model = models.Where(x => x.GetId() == currentModelId).FirstOrDefault();
                //load the selected model to the highlight render
                //leave the model highlighted until a new model is selected or currentmodel == -1
                Console.WriteLine("Select model id :: {0}", currentModelId);
                //for testing the shift to target function
                //targetPosition
                 //Vector3 newTarget = new Vector3(0, 5, 1);
                if(model != null)
                {
                    Vector3 newTarget = model.GetCenter();
                    orbitCamera!.shiftTo(newTarget, 10.0f, Songo.OrbitCamera.SGI.Maths.AnimationMode.BOUNCE);
                    HighLightModel(model);
                }


                //if (toggle)
                //{
                //    orbitCamera!.shiftTo(newTarget, 10.0f, Songo.OrbitCamera.SGI.Maths.AnimationMode.BOUNCE);

                //}
                //else
                //{
                //    orbitCamera!.shiftTo(targetPosition, 10.0f, Songo.OrbitCamera.SGI.Maths.AnimationMode.EASE_IN_OUT);
                //}
                ////toggle = !toggle; 
            }
            else
            {
                Console.WriteLine("Clear highlight render by selecting no model with shift and mouse click");
            }
            return currentModelId;
        }

        public void HighLightModel(MeshObject model)
        {
            selectedModelId = model.GetId();
            highLightModelRenderer = new HighLightModelRenderer(shaderID, model);
        }

        public void HighLightModels(List<MeshObject> selectedModels)
        {
            selectedModelId = -99; // currentModelId is set -1 when more than one model is selected
            highLightModelRenderer = new HighLightModelRenderer(shaderID, selectedModels);
        }

        //public void DeleteHiHighLightModel()
        //{
        //    if (highLightModelRenderer != null)
        //    {
        //        highLightModelRenderer.ClearGPUMemory();
        //    }
        //    highLightModelRenderer = null;
        //}
         

        internal void RendererSetup()
        {
            float eyeDistance = m_sceneMax.Z - m_sceneMin.Z;

            orbitCamera = new OrbitCamera(refGlControl.ClientSize.Width, refGlControl.ClientSize.Height, true);
            targetPosition = new Vector3(m_sceneCenter.X, m_sceneCenter.Y, m_sceneMax.Z);
            cameraPosition = new Vector3(m_sceneCenter.X, m_sceneCenter.Y, m_sceneMax.Z + eyeDistance);

            orbitCamera.lookAt(cameraPosition, targetPosition);

            //----------------------------------------------------------------------------------------------

            framebuffer4ModelPicking = new Framebuffer4ModelPicking();
            screenQuadVAO = framebuffer4ModelPicking.LoadScreenQuad();
            screenShaderID = framebuffer4ModelPicking.LoadScreenQuadShader();

            // frame buffer configuration
            // -------------------------

            framebuffer4ModelPicking.LoadFrameBuffer(refGlControl.ClientSize.Width, refGlControl.ClientSize.Height);
            framebufferId = framebuffer4ModelPicking.GetFramebufferId();
            textureModelColorbuffer = framebuffer4ModelPicking.GetTextureModelColorbufferHandle();
            textureModelColorbufferUnit = framebuffer4ModelPicking.GetTextureModelColorbufferUnit(); 
        }

        protected void InitializeGPUContext()
        {
            // the opengl context must be complete before calling any GL. methods  

            GL.Enable(EnableCap.DebugOutput);

            shader = new ShaderSeparateFiles("Shaders\\OffScreenFramebuffer\\TestVertex.shader", "Shaders\\OffScreenFramebuffer\\TestFragment.shader");
            shader.Bind();
            shaderID = shader.Handle;

            //highLightModelVertexArrayId = MakeHighlightModel(shaderID, meshObjs[0]);

            //ArrayBuffer
            MY.GLCall(vertexArrayId = GL.GenVertexArray());
            GL.BindVertexArray(vertexArrayId); MY.GLCall();

            //VertexBuffer
            VertexBufferId = GL.GenBuffer(); MY.GLCall();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferId); MY.GLCall();
            GL.BufferData(BufferTarget.ArrayBuffer, gpuVertices.Count * ((3 + 4) * sizeof(float) + (1 * sizeof(int))), gpuVertices.ToArray(), BufferUsageHint.StaticDraw); MY.GLCall();
            MY.GLCall();

            //IndexBuffer
            MY.GLCall(indexBufferId = GL.GenBuffer());
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferId); MY.GLCall();
            GL.BufferData(BufferTarget.ElementArrayBuffer, gpuIndexBuffer.Count * sizeof(UInt32), gpuIndexBuffer.ToArray(), BufferUsageHint.StaticDraw); MY.GLCall();
            MY.GLCall();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Color
            GL.EnableVertexAttribArray(1); MY.GLCall();
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, ((3 + 4) * sizeof(float) + (1 * sizeof(int))), 3 * sizeof(float));
            MY.GLCall();

            //position
            GL.EnableVertexAttribArray(0); MY.GLCall();
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, ((3 + 4) * sizeof(float) + (1 * sizeof(int))), 0);
            MY.GLCall();

            //Model id
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribIPointer(2, 1, VertexAttribIntegerType.Int, ((3 + 4) * sizeof(float) + (1 * sizeof(int))), 7 * sizeof(float));
            MY.GLCall();

            GL.BindVertexArray(0);
            shader.Unbind();

            target = Matrix4.LookAt(cameraPosition, new Vector3(m_sceneCenter.X, m_sceneCenter.Y, m_sceneCenter.Z), new Vector3(0f, 1f, 0f));
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), (float)refGlControl.ClientSize.Width / (float)refGlControl.ClientSize.Height, 0.01f, 1000.0f);

            mvp = target * projection;

            //Must remove the forward compatibility flag at the top of this code
            GL.Disable(EnableCap.LineSmooth);

            GL.LineWidth(20);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowBoundingBoxes(bool value)
        {
            if (value == true)
            {
                if (boundBoxRenderer == null)
                {
                    if (models != null)
                    {
                        boundBoxRenderer = new BoundBoxRenderer(shaderID, models);
                    }
                }
            }
            else
            {
                if (boundBoxRenderer != null)
                {
                    boundBoxRenderer.ClearHiHighLightModel();
                }
                boundBoxRenderer = null;
            }
        }


        /// <summary>
        /// Show bounding box for the selected model
        /// </summary>
        public void ShowBoundingBox()
        {
            if (currentModelId != -1 && models != null)
            {
                MeshObject? model = models.Where(x => x.GetId() == currentModelId).FirstOrDefault();
                //load the selected model to the highlight render
                //leave the model highlighted until a new model is selected or currentmodel == -1
                Console.WriteLine("Select model id :: {0}", currentModelId);
                //for testing the shift to target function
                //targetPosition
                //Vector3 newTarget = new Vector3(0, 5, 1);
                if (model != null)
                {
                    Vector3 newTarget = model.GetCenter();
                    orbitCamera!.shiftTo(newTarget, 10.0f, Songo.OrbitCamera.SGI.Maths.AnimationMode.BOUNCE);
                    selectedModelId = model.GetId();
                    boundBoxRenderer = new BoundBoxRenderer(shaderID, model);
                }
            }
        }

        /// <summary>
        /// Tell the Highlight renderer to cleanup all resources
        /// </summary> 
        public void ClearHiHighLightModel()
        {
            if (highLightModelRenderer != null)
            {
                highLightModelRenderer.ClearHiHighLightModel();
            }
            highLightModelRenderer = null;
        }

        internal void WriteGPUBuffers2File(string filename)
        {
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            StringBuilder sb = new StringBuilder();
            //write gpuVertices out to file
            for (int i = 0; i < gpuVertices.Count; i++)
            {
                sb.AppendLine(string.Format("v : [{0}], c : [{1}], i: [{2}]",
                    gpuVertices[i].position.ToString(),
                    gpuVertices[i].color.ToString(),
                    gpuVertices[i].id));
            }
            File.WriteAllText("GPU_Vertices_two", sb.ToString());

            StringBuilder sb1 = new StringBuilder();
            //write gpuVertices out to file
            for (int i = 0; i < gpuIndexBuffer.Count; i++)
            {
                if (i % 10 == 0)
                {
                    sb1.AppendLine();
                }
                sb1.Append(string.Format(" {0} ", gpuIndexBuffer[i]));
            }
            File.WriteAllText(filename, sb1.ToString());
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        }

        public void OnResizeGL(ResizeEventArgs e)
        {
            //base.OnResize(e); 

            if (e.Width < 50 || e.Height < 50) return;

            if (orbitCamera != null)
            {
                orbitCamera.OnResize(e.Width, e.Height);
            }

            screenHeight = e.Height;
            screenWidth = e.Width;

            GL.Viewport(0, 0, screenWidth, screenHeight); MY.GLCall();

            // frame buffer configuration
            // -------------------------
            // Must recreate the quad frame buffer after every screen resize

            if (framebuffer4ModelPicking != null)
            {
                framebuffer4ModelPicking.LoadFrameBuffer(screenWidth, screenHeight);
                framebufferId = framebuffer4ModelPicking.GetFramebufferId();
                textureModelColorbuffer = framebuffer4ModelPicking.GetTextureModelColorbufferHandle();
                textureModelColorbufferUnit = framebuffer4ModelPicking.GetTextureModelColorbufferUnit();
            }
        }

       private  Matrix4 view;
       private int count = 0;
        public void OnRenderFrameGL(double time) //time in milliseconds
        {
            if (orbitCamera == null) return;

            time = time / 1000.00f;  // milliseconds to seconds
                                     //base.OnRenderFrame(e);

            if (DEBUGLOG) Log($"OnRenderFrameGL [delta time (sec)] {time},  count {count++}");

            // update times
            float elapsedTime = orbitCamera.GetTimerElapsed();
            float frameTime = elapsedTime - playTime;
            playTime = elapsedTime;

            if (DEBUGLOG) Log($"Frame time {frameTime}");
            if (DEBUGLOG) Log($"FPS: {1f / frameTime}");
            refMainWindowForm.Text = $"FPS: {1f / frameTime}";
            // update camera
            orbitCamera.update(frameTime);


            view = orbitCamera.getMatrix();
            mvp = view * projection;

            GL.Enable(EnableCap.DepthTest);
            GL.Viewport(0, 0, screenWidth, screenHeight);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);


            // render
            // ------
            // bind to frame-buffer and draw scene as we normally would to color texture 
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebufferId);
            GL.Viewport(0, 0, screenWidth, screenHeight);
            GL.Enable(EnableCap.DepthTest); // enable depth testing (is disabled for rendering screen-space quad)

            // make sure we clear the frame-buffer's content
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            int value = -1;
            //Clear the id color buffer attachment      
            GL.ClearBuffer(ClearBuffer.Color, 1, ref value);

            if (true)
            {
                //shader.use();
                GL.UseProgram(shaderID); 

                GL.BindVertexArray(vertexArrayId);

                GL.EnableVertexAttribArray(0);
                GL.EnableVertexAttribArray(1);
                GL.EnableVertexAttribArray(2);

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferId);

                GL.UniformMatrix4(GL.GetUniformLocation(shader!.Handle, "u_ViewProjection"), false, ref mvp);
                 
                GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill);
                GL.DrawElements(PrimitiveType.Triangles, gpuIndexBuffer.Count, DrawElementsType.UnsignedInt, 0); //MY.GLCall();

                if (true)
                {
                    // Draw the triangle as a wire frame
                    GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line); // Set polygon mode to lines
                    GL.DrawElements(PrimitiveType.Triangles, gpuIndexBuffer.Count, DrawElementsType.UnsignedInt, 0); //MY.GLCall(); 
                    GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill); // Reset to fill (optional) 
                }
            }

            // if there is a current highlight element
            if(highLightModelRenderer != null)
            {
                GL.Disable(EnableCap.DepthTest);
                GL.UseProgram(shaderID);
                // use the same shader for now
                GL.BindVertexArray(highLightModelRenderer.GetVertexArrayId());

                GL.EnableVertexAttribArray(0);
                GL.EnableVertexAttribArray(1);
                GL.EnableVertexAttribArray(2);

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, highLightModelRenderer.GetGPUIndexBufferId());

                GL.UniformMatrix4(GL.GetUniformLocation(shader!.Handle, "u_ViewProjection"), false, ref mvp);
 
                GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill);
                GL.DrawElements(PrimitiveType.Triangles, highLightModelRenderer.GetGPUIndexBufferCount(), DrawElementsType.UnsignedInt, 0); MY.GLCall();
            }

            // if SHow Bounding Box is set to true
            if (boundBoxRenderer != null)
            {
                GL.LineWidth(1);
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                GL.Disable(EnableCap.DepthTest);
                GL.Disable(EnableCap.CullFace);
                GL.UseProgram(shaderID);
                // use the same shader for now
                GL.BindVertexArray(boundBoxRenderer.GetVertexArrayId());

                GL.EnableVertexAttribArray(0);
                GL.EnableVertexAttribArray(1);
                GL.EnableVertexAttribArray(2);

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, boundBoxRenderer.GetGPUIndexBufferId());

                GL.UniformMatrix4(GL.GetUniformLocation(shader!.Handle, "u_ViewProjection"), false, ref mvp);

                GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);
                GL.DrawElements(PrimitiveType.Triangles, boundBoxRenderer.GetGPUIndexBufferCount(), DrawElementsType.UnsignedInt, 0); MY.GLCall();

                //Keep below for 
                if (false)
                {
                    GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill);
                    GL.DrawElements(PrimitiveType.Triangles, boundBoxRenderer.GetGPUIndexBufferCount(), DrawElementsType.UnsignedInt, 0); MY.GLCall();
                }

                GL.Enable(EnableCap.CullFace);
                GL.Disable(EnableCap.Blend);
                //GL.LineWidth(1);
                //GL.Enable(EnableCap.DepthTest);
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);
              
            
            // read the model that is under the cursor at this moment in time
            // write it out to the console window
            Point mpt = System.Windows.Forms.Control.MousePosition;
            mpt = refGlControl.PointToClient(mpt);
            Vector2 pos = new Vector2(mpt.X, mpt.Y);
            
            int pixelData = -1;
            pos.Y = screenHeight - pos.Y;
            GL.ReadPixels((int)pos.X, (int)pos.Y, 1, 1, PixelFormat.RedInteger, PixelType.Int, ref pixelData);

            if (pixelData != currentModelId)
            {
                string? guid = ifcCatalog!.GetGuid(pixelData);
                if (DEBUGLOG) Log(string.Format("Model id: {0} at pixel location [{1},{2}]", pixelData, pos.X, pos.Y));
                if (DEBUGLOG) Log(string.Format("Model id: {0} at pixel location [{1},{2}]", guid, pos.X, pos.Y));
                if (true) Console.WriteLine("Model id: {0} at pixel location [{1},{2}]", pixelData, pos.X, pos.Y);
                if (true) Console.WriteLine("Model id: {0} at pixel location [{1},{2}]", guid, pos.X, pos.Y);

                if (true) Log(string.Format("Model id: {0} at pixel location [{1},{2}]", pixelData, pos.X, pos.Y));
                if (true) Log(string.Format("Model id: {0} at pixel location [{1},{2}]", guid, pos.X, pos.Y));
                currentModelId = pixelData;
            }

            orbitCamera.OnRenderFrame();

            //Bind the default frame buffer for drawing the quad
            //now draw the off screen buffer/image to the screen
            //the off screen buffer is required to get fast model ids under the cursor at runtime

            // now bind back to default frame-buffer and draw a quad plane with the attached frame-buffer color texture
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Viewport(0, 0, screenWidth, screenHeight);
            GL.Disable(EnableCap.DepthTest); // disable depth test so screen-space quad isn't discarded due to depth test.

            GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill);

            // clear all relevant buffers
            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f); // set clear color to white (not really necessary actually, since we won't be able to see behind the quad anyways)
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //screenShader.use();
            GL.UseProgram(screenShaderID);

            GL.BindVertexArray(screenQuadVAO);
            GL.ActiveTexture(TextureUnit.Texture0 + textureModelColorbufferUnit);
            GL.BindTexture(TextureTarget.Texture2D, textureModelColorbuffer);   // use the color attachment texture as the texture of the quad plane
            GL.Uniform1(GL.GetUniformLocation(screenShaderID, "screenTexture"), textureModelColorbufferUnit);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            refGlControl.SwapBuffers();
        } 
        
        public void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            //base.OnFramebufferResize(e);
            screenWidth = e.Width;
            screenHeight = e.Height;

            GL.Viewport(0, 0, screenWidth, screenHeight);
        }

        private void Log(string message)
        {
            LogTextBox.AppendText(message + "\r\n");
        }
    }
}
