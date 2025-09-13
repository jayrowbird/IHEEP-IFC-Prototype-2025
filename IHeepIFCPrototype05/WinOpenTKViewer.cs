// Ignore Spelling: Framebuffer


using HeepWare.IFC.Catalog;
using HeepWare.OBJ.IFC.Library;
using HeepWare.OBJ.Mesh.Data;
using HeepWare.Renderer;
using HeepWare.RTree.Library;
using HeepWare.Windows.Forms.IFC.Prototype;
using MyOpenTK;
using OpenTK.GLControl;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Songo.OrbitCamera;
using Songo.OrbitCamera.SGI.Maths;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WinFormsConsoleReadonly;
using Keys = OpenTK.Windowing.GraphicsLibraryFramework.Keys;

namespace ViewSeparation_PickingWithOrbitCamera
{
    public class WinOpenTKViewer
    {
        private readonly bool DEBUGLOG = false;
        private GLControl glControl;
        private TextBox LogTextBox;
        private IheepIFCPrototypeForm mainWindowForm;

        // The glControl did not trigger key board events
        // so i chose to use the glcontrol native input interface  
        private INativeInput? _nativeInput = null;

        IFCCatalog ifcCatalog = IFCCatalog.Instance;

        // Added for finding connected meshes
        private List<MeshObject> models;
        private RTreeLibrary rtreeLibrary;


        // Below the Opengl api debug call back delegate 
        private static DebugProc DebugMessageDelegate = OnDebugMessage;

        private System.Windows.Forms.Timer rendererTimer;
        private Stopwatch rendererClock;

        private int screenWidth;
        private int screenHeight;

        //Added for orbit camera
        private SGIMath sgimath = new SGIMath();
        // global variables

        private bool mouseRightDown = false;
        private bool mouseLeftDown = false;
        private bool plusKeyDown = false;
        private bool minusKeyDown = false;
        //shift key down mouse click selects element
        private bool shiftKeyDown = false;
        private bool leftKeyDown = false;
        private bool rightKeyDown = false;
        private bool upKeyDown = false;
        private bool downKeyDown = false;
        private float mouseX, mouseY = float.MinValue;
        private float mouseDownX, mouseDownY = float.MinValue;
        private int drawMode;

        //Camera variables
        private Vector3 cameraAngle;
        private OrbitCamera? orbitCamera;
        private Quaternion cameraQuat;

        private bool DEBUGFLAG = false;
        List<MeshObject>? meshObjs;


        FastRenderer? fastRenderer = null;

        private int currentModelId = -1;

        //public WinOpenTKViewer(Form1 mainWindowForm_)IheepIFCPrototype
        public WinOpenTKViewer(IheepIFCPrototypeForm mainWindowForm_)
        {
            mainWindowForm = mainWindowForm_;
            LogTextBox = mainWindowForm.LogTextBox;
            glControl = mainWindowForm.glControl1;

            //Set initial OpenTK viewer height and width
            screenWidth = glControl.Width;
            screenHeight = glControl.Height;

            //// must use the native input controls because the win forms
            //// input events do work for the game engine window

            InitNativeInputs();

            //Game Clock running continually
            rendererClock = new Stopwatch();
            rendererClock.Start();

            //Render event generator
            rendererTimer = new System.Windows.Forms.Timer();
            rendererTimer.Tick += (sender, e) => { glControl.Invalidate(); };
            rendererTimer.Interval = 50;
            rendererTimer.Start();

            glControl.Paint += GlControl_Paint;
            glControl.Resize += GlControl_Resize;

            // Log any focus changes for debugging.
            if (true)
            {
                glControl.GotFocus += (sender, e) =>
                 { if (true) Log("Focus in"); };
                glControl.LostFocus += (sender, e) =>
                {
                    if (DEBUGLOG) Log("Focus out");
                };
            }

            GL.DebugMessageCallback(DebugMessageDelegate, IntPtr.Zero);
            GL.Enable(EnableCap.DebugOutput);
        }
        /// <summary>
        /// Use this renderer for now 
        /// this renderer only needs position color and id for each gpu vertex
        /// </summary>
        /// <param name="filename"></param>
        public void LoadFile3(string filename)
        {
            FastOBJFileLoad fastOBJFileLoad = new FastOBJFileLoad();
            fastOBJFileLoad.LoadFile(filename);



            List<VertexPosColorId> gpuVertices = fastOBJFileLoad.GetGPUVertexBuffer();
            List<uint> gpuIndexBuffer = fastOBJFileLoad.GetGPUIndexBuffer();

            fastRenderer = new FastRenderer(mainWindowForm, gpuVertices, gpuIndexBuffer);

            //for testing
            //-------------------------------------------------------------
            // must merge FastOBJFileLoad with IFCOBJLib2
            IFCOBJLib2 ifcOBJLib2 = new IFCOBJLib2(filename);
            ifcOBJLib2.LoadFile();
            fastRenderer.SetModels(ifcOBJLib2.GetMeshObjects());

            models = ifcOBJLib2.GetMeshObjects();
            fastRenderer.HighLightModel(models[0]);
            //-------------------------------------------------------------

            orbitCamera = fastRenderer.GetOrbitCamera();

            //glControl.Focus();
            OnResizeGL(new ResizeEventArgs(screenWidth = glControl.Width, screenHeight = glControl.Height));
        }

        public void SelectCurrentModel()
        {
            if (fastRenderer != null)
            {
                SelectCurrentModel();
                currentModelId = fastRenderer.GetSelectedModelId();
            }
        }

        /// <summary>
        /// Called by the win forms tool menu item click
        /// </summary>
        public void ClearHiHighLightModel()
        {
            if (fastRenderer != null)
            {
                fastRenderer.ClearHiHighLightModel();
            }
            mainWindowForm.SetClearSelectedEnabledBool(false);
            mainWindowForm.SetFindConnectionsEnabledBool(false);
        }

        /// <summary>
        /// Called by the win forms tool menu item click
        /// </summary>
        public void ShowBoundingBoxes(bool value)
        {
            fastRenderer.ShowBoundingBoxes(value);
        }

        /// <summary>
        /// Called by the win forms tool menu item click
        /// </summary>
        public void FindConnections()
        {
            if (fastRenderer != null)
            {
                rtreeLibrary = new RTreeLibrary();
                rtreeLibrary.Initialize();
                rtreeLibrary.LoadMeshObjects(models);
                int selectedModelId = fastRenderer.GetSelectedModelId();
                MeshObject? meshObject = models.Where(x => x.GetId() == selectedModelId).FirstOrDefault();
                List<Connection> connections;
                List<MeshObject> highlightModels = new List<MeshObject>();
                if (meshObject != null)
                {
                    connections = rtreeLibrary.Search(meshObject);
                    if (connections != null && connections.Count > 0)
                    {
                        for (int i = 0; i < connections.Count; i++)
                        {
                            MeshObject? mOject = models.Where(x => x.GetName() == connections[i].meshName).FirstOrDefault();
                            if (mOject != null)
                            {
                                highlightModels.Add(mOject);
                            }
                        }
                    }
                    if (highlightModels.Count > 0)
                    {
                        fastRenderer.HighLightModels(highlightModels);
                    }
                    else
                    {
                        int rtreeCount = rtreeLibrary.GetRTreeCount();
                    }
                }
            }
        }

        private void InitNativeInputs()
        {
            glControl.Focus();

            _nativeInput = glControl.EnableNativeInput();

            _nativeInput.KeyDown += _nativeInput_KeyDown;
            _nativeInput.KeyUp += _nativeInput_KeyUp;

            _nativeInput.MouseDown += _nativeInput_MouseDown;
            _nativeInput.MouseUp += _nativeInput_MouseUp;

            _nativeInput.MouseWheel += _nativeInput_MouseWheel;

            _nativeInput.MouseMove += _nativeInput_MouseMove;

            _nativeInput.MouseEnter += _nativeInput_MouseEnter;
        }

        private void _nativeInput_MouseEnter()
        {
            glControl.Focus();
        }

        private void _nativeInput_MouseWheel(MouseWheelEventArgs obj)
        {
            OnMouseWheelGL(obj);
        }

        private void _nativeInput_KeyUp(KeyboardKeyEventArgs obj)
        {
            OnKeyUpGL(obj);
        }

        protected void _nativeInput_KeyDown(KeyboardKeyEventArgs obj)
        {
            if (DEBUGLOG) Log($"  Key Down");

            OnKeyDownGL(obj);
        }

        private void _nativeInput_MouseUp(MouseButtonEventArgs obj)
        {
            OnMouseUpGL(obj);
        }

        private void _nativeInput_MouseDown(MouseButtonEventArgs obj)
        {
            OnMouseDownGL(obj);
        }

        private void _nativeInput_MouseMove(MouseMoveEventArgs obj)
        {
            OnMouseMoveGL(obj);
        }

        protected void OnResizeGL(ResizeEventArgs e)
        {
            //base.OnResize(e); 
            if (fastRenderer != null)
                fastRenderer.OnResizeGL(e);
            return;
        }
        protected void OnRenderFrameGL(double time) //time in milliseconds
        {
            if (fastRenderer == null) return;
            fastRenderer.OnRenderFrameGL(time);
            return;
        }

        protected void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            //base.OnFramebufferResize(e);
            if (fastRenderer != null)
                fastRenderer.OnFramebufferResize(e);
            return;
        }

        protected void OnKeyUpGL(KeyboardKeyEventArgs e)
        {
            //base.OnKeyUp(e);

            if (e.Key == Keys.Equal || e.Key == Keys.KeyPadAdd)
            {
                plusKeyDown = false;
                if (orbitCamera != null)
                {
                    orbitCamera.stopForward();
                    if (DEBUGFLAG) Console.WriteLine("main::orbitCamera.stopForward");
                }
                else
                {
                    throw new Exception("Orbit Camera is null");
                }
            }
            else if (e.Key == Keys.LeftShift || e.Key == Keys.RightShift)
            {
                shiftKeyDown = false;
                return;
            }
            else if (e.Key == Keys.Minus)
            {
                minusKeyDown = false;
                if (orbitCamera != null)
                {
                    orbitCamera.stopForward();
                    if (DEBUGFLAG) Console.WriteLine("main::orbitCamera.stopForward");
                }
                else
                {
                    throw new Exception("Orbit Camera is null");
                }
            }
        }
        protected void OnKeyDownGL(KeyboardKeyEventArgs e)
        {
            //base.OnKeyDown(e); 
            if (e.Key == Keys.Escape)
            {
                //this.Close();
                return;
            }
            else if (e.Key == Keys.LeftShift || e.Key == Keys.RightShift)
            {
                shiftKeyDown = true;
                return;
            }
            else if (e.Key == Keys.Space)
            {
                return;
            }
            else if (e.Key == Keys.R)
            {
                if (orbitCamera != null)
                {
                    orbitCamera.resetCamera();
                }
                else
                {
                    throw new Exception("Orbit Camera is null");
                }
                return;
            }
            else if (e.Key == Keys.D)
            {
                drawMode = ++drawMode % 3;
                if (orbitCamera != null)
                {
                    orbitCamera.SetDrawingMode(drawMode);
                }
                else
                {
                    throw new Exception("Orbit Camera is null");
                }
                return;
            }
            else if (e.Key == Keys.Equal || e.Key == Keys.KeyPadAdd)
            {
                //camera.moveForward( 1.0f);
                //camera.moveForward( 1.0f, 0.5f, Gil::EASE_OUT);
                if (!plusKeyDown)
                {
                    plusKeyDown = true;
                    if (orbitCamera != null)
                    {
                        orbitCamera.startForward(SGIMath.MOVE_SPEED, SGIMath.MOVE_ACCEL);
                        if (DEBUGFLAG) Console.WriteLine("main::orbitCamera.startForward");
                    }
                    else
                    {
                        throw new Exception("Orbit Camera is null");
                    }
                }
                return;
            }
            else if (e.Key == Keys.Minus)
            {
                //camera.moveForward( 1.0f);
                //camera.moveForward( 1.0f, 0.5f, Gil::EASE_OUT);
                if (!minusKeyDown)
                {
                    minusKeyDown = true;
                    if (orbitCamera != null)
                    {
                        orbitCamera.startForward(-SGIMath.MOVE_SPEED, SGIMath.MOVE_ACCEL);
                    }
                    else
                    {
                        throw new Exception("Orbit Camera is null");
                    }
                }
                return;
            }
        }

        protected void OnMouseWheelGL(MouseWheelEventArgs e)
        {
            if (e.Offset.Y > 0)
            {
                if (orbitCamera != null)
                {
                    if (DEBUGFLAG) Console.WriteLine(" {0}  e.Offset.Y > 0", e.OffsetY);
                    orbitCamera.moveForwardPercent(-.15f, .5f, AnimationMode.EASE_OUT);
                }
                else
                {
                    throw new Exception("Orbit Camera is null");
                }
            }
            else if (e.Offset.Y < 0)
            {
                if (orbitCamera != null)
                {
                    if (DEBUGFLAG) Console.WriteLine(" {0}  e.Offset.Y < 0", e.OffsetY);
                    orbitCamera.moveForwardPercent(.15f, .5f, AnimationMode.EASE_OUT);
                }
                else
                {
                    throw new Exception("Orbit Camera is null");
                }
            }
        }
        protected void OnMouseUpGL(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                this.mouseLeftDown = false;
            }
            else if (e.Button == MouseButton.Right)
            {
                this.mouseRightDown = false;
            }
        }











        // Create a timer with a two second interval.
        System.Windows.Forms.Timer? aTimer;
        // Hook up the Elapsed event for the timer. 



        private int ClickCount = 0;
        private int TickCount = 0;
        private OpenTK.Windowing.Common.MouseButtonEventArgs? refArgs;
        protected void OnMouseDownGL(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Right)
            {
                OnMouseDownSingleGL(e);
            }

            ClickCount++;
            if (ClickCount == 1)
            {
                refArgs = e;
                aTimer = new System.Windows.Forms.Timer();
                aTimer.Tick += ATimer_Tick;
                aTimer.Interval = (int)(SystemInformation.DoubleClickTime * 0.8);
                aTimer.Enabled = true;
            }
            Console.WriteLine("Click count {0}", ClickCount);
        }

        private void ATimer_Tick(object? sender, EventArgs e)
        {
            Console.WriteLine("Tick count {0}", TickCount++);
            Console.WriteLine("Click count {0}", ClickCount);

            if (TickCount > 0)
            {
                if (ClickCount < 2)
                {
                    if ((Control.MouseButtons & MouseButtons.Left) != 0)
                    {
                        //mouse button must be down
                        // do nothing if a single click is detected

                        Console.WriteLine("Click count {0}", ClickCount);
                        OnMouseDownSingleGL((OpenTK.Windowing.Common.MouseButtonEventArgs)refArgs); // Implement your single-click logic
                    }
                }
                else
                {
                    Console.WriteLine("Click count {0}", ClickCount);
                    OnMouseDownDoubleGL((OpenTK.Windowing.Common.MouseButtonEventArgs)refArgs); // Implement your double-click logic
                }
                //Cleanup below
                refArgs = null;
                TickCount = 0;
                ClickCount = 0;
                if (aTimer != null)
                {
                    aTimer.Tick -= ATimer_Tick;
                    aTimer = null;
                }
            }
        }

        protected void OnMouseDownDoubleGL(MouseButtonEventArgs e)
        {
            //Set menu item tool -> Find connections active
            mainWindowForm.SetClearSelectedEnabledBool(true);
            mainWindowForm.SetFindConnectionsEnabledBool(true);
            if (fastRenderer != null)
            {
                //set the current selected model to be highlighted and moved to the center of the screen
                int currentModelId = fastRenderer.SelectCurrentModel();

                if (currentModelId < 0) return;


                //if non ifc file was opened there will be nothing stored in the jsontree
                if (mainWindowForm.JsonTreeView.Nodes.Count > 0)
                { 
                    string? searchTag = ifcCatalog.GetGuid(currentModelId);
                    if (searchTag != null)
                    {
                        //------------------------------------------------------------------------------------------
                        // set the current model properties in the node view
                        TreeNode? resultNode = null;
                        foreach (TreeNode rootNode in mainWindowForm.JsonTreeView.Nodes)
                        {
                            resultNode = FindNodeByTag(searchTag, rootNode.Nodes);
                            if (resultNode != null)
                            {
                                break; // Node found, exit the loop
                            }
                        }

                        if (resultNode != null)
                        {
                            mainWindowForm.JsonTreeView.SelectedNode = resultNode;
                            mainWindowForm.JsonTreeView.Focus();
                        }
                        else
                        {
                            MessageBox.Show("Node not found in tree", "did not work");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to find GUID from current model id");
                    }
                }
                //------------------------------------------------------------------------------------------
            }
        }

        private TreeNode? FindNodeByTag(object tag, TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Tag != null && node.Tag.Equals(tag))
                {
                    return node;
                }

                // Recursively search child nodes
                TreeNode foundNode = FindNodeByTag(tag, node.Nodes);
                if (foundNode != null)
                {
                    return foundNode;
                }
            }
            return null; // Node not found
        }

        protected void OnMouseDownSingleGL(MouseButtonEventArgs e)
        {
            //element selection
            if (shiftKeyDown == true)
            {
                //tell the render to select the current element
                fastRenderer!.SelectCurrentModel();
                return;
            }
            //Convert cursor to mouse position relative to the GLControl

            //Vector2 mouse = glControl.GetMouseState().Position;
            Point mpt = Control.MousePosition;
            mpt = glControl.PointToClient(mpt);
            //mpt.Y = glControl.Height - mpt.Y;

            Log($"Mouse Down position {mpt.ToString()}");

            //float x = mouse.X;
            //float y = mouse.Y;

            float x = mpt.X;
            float y = mpt.Y;

            mouseX = x;
            mouseY = y;

            //if (button == GLUT_LEFT_BUTTON)
            if (e.Button == MouseButton.Left)
            {
                if (e.IsPressed)
                {
                    if (orbitCamera != null)
                    {
                        cameraAngle = orbitCamera.getAngle(); // get current camera angle
                        cameraQuat = orbitCamera.getQuaternion();

                        mouseDownX = x;
                        mouseDownY = y;
                        mouseLeftDown = true;
                    }
                    else
                    {
                        throw new Exception("Orbit Camera is null");
                    }
                }
            }
            else
            {
                this.mouseLeftDown = false;
            }

            if (e.Button == MouseButton.Right)
            {
                if (e.IsPressed)
                {
                    mouseDownX = x;
                    mouseDownY = y;
                    this.mouseRightDown = true;
                }
            }
            else
            {
                this.mouseRightDown = false;
            }
        }

        protected void OnMouseMoveGL(MouseMoveEventArgs e)
        {
            const float SCALE_ANGLE = 0.2f;
            const float SCALE_SHIFT = 0.2f;
            float prevX = mouseX;
            float prevY = mouseY;

            //Vector2 mouse = glControl.GetMouseState().Position;
            Point mpt = System.Windows.Forms.Control.MousePosition;
            mpt = glControl.PointToClient(mpt);

            float x = mpt.X;
            float y = mpt.Y;

            if (DEBUGLOG) Log($"glControl Mouse Position : {x}, {y}");
            if (true) Log($"Form Mouse Position : {e.X}, {e.Y}");

            mouseX = x;
            mouseY = y;

            if (this.mouseLeftDown)
            {
                Vector3 delta;
                delta.Y = (mouseX - mouseDownX) * SCALE_ANGLE;
                delta.X = (mouseY - mouseDownY) * SCALE_ANGLE;

                // re-compute camera matrix
                /*
                //@@ using Euler angles
                Vector3 angle;
                angle.x = cameraAngle.x + delta.x;
                angle.y = cameraAngle.y - delta.y;  // must negate for camera
                //camera.rotateTo(angle);
                camera.rotateTo(angle, 0.5f, Gil::EASE_OUT);
                */

                //@@ using quaternion
                Quaternion qx = sgimath.QuaternionFromVector(new Vector3(1, 0, 0), delta.X * SGIMath.DEG2RAD * 0.5f);   // rotate along X-axis
                Quaternion qy = sgimath.QuaternionFromVector(new Vector3(0, 1, 0), delta.Y * SGIMath.DEG2RAD * 0.5f);   // rotate along Y-axis
                Quaternion q = qx * qy * cameraQuat;
                //Quaternion q = Quaternion::getQuaternion(delta * DEG2RAD * 0.5f); // quat from angles
                //q *= cameraQuat;
                //camera.rotateTo(q);
                if (orbitCamera != null)
                {
                    orbitCamera.rotateTo(q, 0.5f, AnimationMode.EASE_OUT);
                }
                else
                {
                    throw new Exception("Orbit Camera is null");
                }
                /*
                //@@ using delta angle
                //camera.rotate(delta);
                //camera.rotate(delta, 0.5f, Gil::EASE_OUT);
                */
            }
            if (this.mouseRightDown)
            {
                Vector2 delta;
                delta.X = (mouseX - prevX) * SCALE_SHIFT;
                delta.Y = (mouseY - prevY) * SCALE_SHIFT;
                if (orbitCamera != null)
                {
                    orbitCamera.shift(delta, 0.5f, AnimationMode.EASE_OUT);
                }
                else
                {
                    throw new Exception("Orbit Camera is null");
                }
            }
        }
        private void Log(string message)
        {
            LogTextBox.AppendText(message + "\r\n");
        }

        private void GlControl_Resize(object? sender, EventArgs e)
        {
            glControl.MakeCurrent();
            OnResizeGL(new ResizeEventArgs(glControl.ClientSize.Width, glControl.ClientSize.Height));
        }

        private void GlControl_Paint(object? sender, PaintEventArgs e)
        {
            glControl.MakeCurrent();
            //Convert milliseconds to seconds
            OnRenderFrameGL(rendererClock.ElapsedMilliseconds);
        }

        private static void OnDebugMessage(
                DebugSource source,     // Source of the debugging message.
                DebugType type,         // Type of the debugging message.
                int id,                 // ID associated with the message.
                DebugSeverity severity, // Severity of the message.
                int length,             // Length of the string in pMessage.
                IntPtr pMessage,        // Pointer to message string.
                IntPtr pUserParam)      // The pointer you gave to OpenGL, explained later.
        {
            // In order to access the string pointed to by pMessage, you can use Marshal
            // class to copy its contents to a C# string without unsafe code. You can
            // also use the new function Marshal.PtrToStringUTF8 since .NET Core 1.1.
            string message = Marshal.PtrToStringAnsi(pMessage, length);

            // The rest of the function is up to you to implement, however a debug output
            // is always useful.
            Console.WriteLine("[{0} source={1} type={2} id={3}] {4}", severity, source, type, id, message);

            // Potentially, you may want to throw from the function for certain severity
            // messages.
            if (type == DebugType.DebugTypeError)
            {
                throw new Exception(message);
            }
        }
    }
}
