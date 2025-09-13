// Ignore Spelling: Iheep Json

using HeepWare.Data;
using HeepWare.Data.Project;
using HeepWare.Data.XML.Load.EntryPoint;
using HeepWare.Windows.Forms.IFC.Prototype;
using JsonTreeView;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ViewSeparation_PickingWithOrbitCamera;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinFormsConsoleReadonly
{
    //Call backs to fix cross thread problem with the forms UI thread
    public delegate void SetSelectedTabCallback(string value);
    public delegate void SetLoadJsonTreeCallback(string value);

    public partial class IheepIFCPrototypeForm : Form
    {
        //Below is a crappy solution to getting back onto the ui thread 
        //from the external processes
        private System.Windows.Forms.Timer _timer;

        //below for the slide out info panel
        private int PW = -1;
        private bool hidden = false;

        private WinOpenTKViewer? winOpenTKViewer;
        private bool SHOWBOUNDINGBOXES = false;

        //Below is used calling ifcConvert.exe as an external process
        private IFCProcessingManager processingManager;
        private Process objConvertProcessRef;

        private bool? complete = null;
        public IheepIFCPrototypeForm()
        {
            InitializeComponent();

            tabControl1.SelectedTab = tabControl1.TabPages["tabPageViewer"];
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;

            //glcontrol needs to be shown so it can be initialized without errors


            // Pass references to the OBJ file processing results text box and XML file processing results
            processingManager = new IFCProcessingManager(textBoxOBJProcessingText, textBoxXMLProcessingText);

            //This setting the info window width and makes sure it is shown
            PW = ProcessInfoTextBox.Width;
            hidden = false;

            //the form must be fully initialized or the glcontrol will fail 
            this.Shown += new System.EventHandler(this.FormViewer_Shown);
        }


        private void TabControl1_SelectedIndexChanged(object? sender, EventArgs e)
        {
            TabControl tabControl = (TabControl)sender;
            if (tabControl != null)
            {
                if (tabControl.SelectedTab.Name == "tabPageOBJconversion")
                {
                    ShowHide.Visible = true;
                }
                else
                {
                    ShowHide.Visible = false;
                }
            }
        }

        //timer to pole the external process completion status
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Update the label with the current time
            if (complete != null)
            {
                InitGLControl((bool)complete);
                _timer.Stop();
                _timer = null;
            }
        }

        private void FormViewer_Shown(object? sender, EventArgs e)
        {
            winOpenTKViewer = new WinOpenTKViewer(this);
            tabControl1.SelectedTab = tabControl1.TabPages["tabPageOBJconversion"];
        }

        private void IheepIFCPrototype_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (processingManager.IsDisposed() != true)
            {
                processingManager.Dispose();
            }
        }

        private void ShowHide_Click(object sender, EventArgs e)
        {
            if (hidden == true)
            {
                ShowHide.Text = ">";
            }
            else
            {
                ShowHide.Text = "<";
            }
            timer2.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (hidden)
            {
                ProcessInfoTextBox.Width += 20;
                if (ProcessInfoTextBox.Width >= PW)
                {
                    timer2.Stop();
                    hidden = false;
                    this.Refresh();
                }
            }
            else
            {
                ProcessInfoTextBox.Width -= 20;
                if (ProcessInfoTextBox.Width <= 0)
                {
                    timer2.Stop();
                    hidden = true;
                    this.Refresh();
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this,
                "This demonstrates a simple use of the new OpenTK 4.x GLControl.",
                "GLControl Test Form",
                MessageBoxButtons.OK);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string selectedFilename = string.Empty;

            tabControl1.SelectedTab = tabControl1.TabPages["tabPageOBJconversion"];

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "ifc files (*.ifc)|*.ifc|Wavefront_obj files (*.obj)|*.obj|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = false;



                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Crappy solution to get back on the UI thread after the external process complete
                    //I use this timer to check the complete status of the background threads
                    //the timer is started when the external xml convert process completes
                    _timer = new System.Windows.Forms.Timer();
                    _timer.Interval = 500;
                    _timer.Tick += timer1_Tick;

                    //Get the path of specified file
                    string filePath = openFileDialog.FileName;
                    if (filePath != null && filePath.ToLower().EndsWith(".ifc"))
                    {
                        string path = Path.GetFullPath(filePath);
                        string dirPath = Path.GetDirectoryName(filePath);

                        string filename = Path.GetFileNameWithoutExtension(filePath);
                        selectedFilename = Path.Combine(dirPath, filename);

                        processingManager.SetIFCFullFilename(selectedFilename);
                        tabControl1.SelectedTab = tabControl1.TabPages["tabPageOBJconversion"];

                        objConvertProcessRef = processingManager.Convert2OBJ();
                        objConvertProcessRef.Exited += ObjConvertProcess_Exited;

                        _timer.Start();
                    }
                    else if (filePath != null && filePath.ToLower().EndsWith(".obj"))
                    { 
                        //select the viewer tab 
                        //Hide the info window
                        hidden = true;
                        ProcessInfoTextBox.Width = 0;
                        ShowHide_Click((null), null);

                        //Select the IFC->wavefront obj file viewer tab
                        SetSelectedTab("tabPageViewer");

                        //Load Wavefront obj file for viewing
                        LogTextBox.AppendText("\n ToDo add code to load obj files");

                        if (winOpenTKViewer != null)
                        {
                            winOpenTKViewer.LoadFile3(filePath);
                        }
                    }
                    else
                    {
                        throw new Exception("unknown file type selected");
                    }
                }
            }
        }


        private void SetSelectedTab(string tabname)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.tabControl1.InvokeRequired)
            {
                SetSelectedTabCallback d = new SetSelectedTabCallback(SetSelectedTab);
                this.Invoke(d, new object[] { tabname });
            }
            else
            {
                tabControl1.SelectedTab = tabControl1.TabPages[tabname];
                if (tabname == "tabPageXMLconversion")
                {
                    Process xmlConvertProcess = processingManager.Convert2XML();
                    xmlConvertProcess.Exited += XmlConvertProcess_Exited;
                }
            }
        }

        internal void InitGLControl(bool value)
        {
            if (value == true)
            {
                if (winOpenTKViewer != null)
                {
                    //select the viewer tab 
                    //Hide the info window
                    hidden = true;
                    ProcessInfoTextBox.Width = 0;
                    ShowHide_Click((null), null);

                    //Load Wavefront obj file for viewing
                    LogTextBox.AppendText("\n ToDo add code to select the selected node by id");

                    //Select the IFC->wavefront obj file viewer tab
                    SetSelectedTab("tabPageViewer");

                    winOpenTKViewer.LoadFile3(processingManager.IFCobjFullFilename);
                }
            }
            else
            {
                throw new Exception("Could not initialize GL control");
            }
        }

        private void ObjConvertProcess_Exited(object? sender, EventArgs e)
        {
            //tabControl1.SelectedTab = tabControl1.TabPages["tabPageXMLconversion"];
            SetSelectedTab("tabPageXMLconversion");

        }

        private void LoadJsonTree(string jsonString)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.tabControl1.InvokeRequired)
            {
                SetLoadJsonTreeCallback d = new SetLoadJsonTreeCallback(LoadJsonTree);
                this.Invoke(d, new object[] { jsonString });
            }
            else
            {
                //Hide the info window
                hidden = true;
                ProcessInfoTextBox.Width = 0;
                ShowHide_Click((null), null);

                //Select the IFC->wavefront obj file viewer tab
                SetSelectedTab("tabPageViewer");
                JsonTreeView.LoadJsonToTreeView(jsonString);
                JsonTreeView.ExpandAll();
                JsonTreeView.SelectedNode = JsonTreeView.Nodes[0];
            }
        }

        private void XmlConvertProcess_Exited(object? sender, EventArgs e)
        {
            // Process to load xml data by converting it to json
            //---------------------------------------------------------------------------------
            MainEntryPoint mainEntryPoint = new MainEntryPoint(processingManager.IFCxmlFullFilename);

            IFCProject ifcProject = mainEntryPoint.GetIFCProject();
            string rootID = ifcProject.GetRootElementId();
            IFCElement? xml_ifc_properties__all = ifcProject.LoadElementWithChildren2(rootID);
            PrintChildernNames(xml_ifc_properties__all);
            string jsonString = JsonConvert.SerializeObject(xml_ifc_properties__all);
            //---------------------------------------------------------------------------------
            //string jsonString = File.ReadAllText("TestData\\ExampleIFCJson.json");
            //JsonTreeView.LoadJsonToTreeView(jsonString);
            //---------------------------------------------------------------------------------

            LoadJsonTree(jsonString);

            if (processingManager.IFCxmlFullFilename != null)
            {
                if (File.Exists(processingManager.IFCxmlFullFilename) == true)
                {
                    complete = (true);
                }
                else
                {
                    complete = (false);
                }
            }
        }

        /// <summary>
        /// when find connections is clicked when a mesh is already selected
        /// try to find all connected meshes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FindConnectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Mesh  must be selected, used the selected mesh to start the range tree search
            //if connections are found send the list of found meshed to the High light renderer
            //winOpenTKViewer.
            //throw new Exception("finish adding rtree library");
            if (winOpenTKViewer != null)
            {
                // this also disables the menu tool items
                winOpenTKViewer.FindConnections();
            }

        }

        internal void SetClearSelectedEnabledBool(bool value)
        {
            ClearSelectionToolStripMenuItem.Enabled = value;
        }

        internal void SetFindConnectionsEnabledBool(bool value)
        {
            FindConnectionsToolStripMenuItem.Enabled = value;
        }

        /// <summary>
        /// Clear all high lighted meshes and cleanup GPU resources
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (winOpenTKViewer != null)
            {
                // this also disables the menu tool items
                winOpenTKViewer.ClearHiHighLightModel();
            }
        }
         

        private static void PrintChildernNames(IFCElement elem)
        {
            if (elem == null) return;
            string indent = "";
            Console.WriteLine("{2} {3} Elem name {0}, {1}", elem.ifcName, elem.Id, indent, totalelems);
            totalelems++;
            if (elem.children != null && elem.children.Count > 0)
            {
                for (int i = 0; i < elem.children.Count; i++)
                {
                    printChildren(elem.children[i], indent, i);
                }
            }
        }
        static int totalelems = 0;
        private static void printChildren(IFCElement elem, string indent, int index)
        {
            indent += "\t";
            Console.WriteLine("{2} {3} {4} Elem name {0}, {1}", elem.ifcName, elem.Id, indent, index, totalelems);
            totalelems++;
            if (elem.children != null && elem.children.Count > 0)
            {
                IFCElement element;
                for (int i = 0; i < elem.children.Count; i++)
                {
                    element = elem.children[i];
                    Console.WriteLine("{2} {3} {4} Elem name {0}, {1}", element.ifcName, element.Id, indent, i, totalelems);
                    totalelems++;
                    printChildren(element, indent, i);
                }
            }
        }

        private void ShowBoundingBoxesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SHOWBOUNDINGBOXES = !SHOWBOUNDINGBOXES;
            if (winOpenTKViewer != null)
            {
                // this also disables the menu tool items
                winOpenTKViewer.ShowBoundingBoxes(SHOWBOUNDINGBOXES);
            }
        }
    }
}