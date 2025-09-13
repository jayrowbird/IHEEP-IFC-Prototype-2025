// Ignore Spelling: Json

using HeepWare.Data;
using HeepWare.Data.Project;
using HeepWare.Data.XML.Load.EntryPoint;
using JsonTreeView;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace HeepWare.Windows.Forms.IFC.Prototype
{
    public delegate void SetSelectedTabCallback(string value); 
    public delegate void SetLoadJsonTreeCallback(string value);
    public partial class Form1 : Form
    {
        IFCProcessingManager processingManager;
        Process objConvertProcessRef;

        public Form1()
        {
            InitializeComponent();
            // Pass references to the OBJ file processing results text box and XML file processing results
            processingManager = new IFCProcessingManager(textBoxOBJProcessingText, textBoxXMLProcessingText);

            tabControl1.TabPages[0].Width += 50;
            tabControl1.TabPages[1].Width += 50;
            tabControl1.TabPages[2].Width += 50;

            //Text describing the process used in this application
            InitializeText(); 
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (processingManager.IsDisposed() != true)
            {
                processingManager.Dispose();
            }
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string selectedFilename = string.Empty;

            tabControl1.SelectedTab = tabControl1.TabPages["tabPageOBJconversion"];

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "ifc files (*.ifc)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = false;



                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    string filePath = openFileDialog.FileName;

                    string path = Path.GetFullPath(filePath);
                    string dirPath = Path.GetDirectoryName(filePath);

                    string filename = Path.GetFileNameWithoutExtension(filePath);
                    selectedFilename = Path.Combine(dirPath, filename);

                    processingManager.SetIFCFullFilename(selectedFilename);
                    tabControl1.SelectedTab = tabControl1.TabPages["tabPageOBJconversion"];

                    objConvertProcessRef = processingManager.Convert2OBJ( );
                    objConvertProcessRef.Exited += ObjConvertProcess_Exited;
                     
                    
                    //////Make tab2 active
                    //tabControl1.SelectedTab = tabControl1.TabPages["tabPageXMLconversion"];
                    //Process xmlConvertProcess = processingManager.Convert2XML();
                    //xmlConvertProcess.Exited += XmlConvertProcess_Exited;


                    //// Process to load xml data by converting it to json
                    ////---------------------------------------------------------------------------------
                    //MainEntryPoint mainEntryPoint = new MainEntryPoint(processingManager.IFCxmlFullFilename);

                    //IFCProject ifcProject = mainEntryPoint.GetIFCProject();
                    //IFCElement? elem_all = ifcProject.LoadElementWithChildren2("0DNWI2k6L6CglTxC23nIIJ");
                    //PrintChildernNames(elem_all);
                    ////---------------------------------------------------------------------------------
                    //string jsonString = File.ReadAllText("TestData\\ExampleIFCJson.json");
                    //JsonTreeView.LoadJsonToTreeView(jsonString);
                    ////---------------------------------------------------------------------------------
 
                    ////RunConverter2OBJ(selectedFilename);
                    //////Make tab2 active
                    ////tabControl1.SelectedTab = tabControl1.TabPages["tabPage2"];
                    ////RunConverter2XML(selectedFilename);
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
                textBox1.Hide();
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
        }

        //private void LoadWAveFrontFile4Viewing(string jsonString)
        //{
        //    // to do load converted obj file here
        //    //tabControl1.SelectedTab = tabControl1.TabPages["tabPageViewer"];
        //    SetSelectedTab("tabPageViewer");
        //    JsonTreeView.LoadJsonToTreeView(jsonString);
        //    //throw new Exception("made it to the viewer tab");
        //}

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

        private void InitializeText()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Process used to load an IFC file");
            sb.AppendLine();
            sb.AppendLine("This application uses the Building Smart");
            sb.AppendLine("IFCConvert.exe to split the selected IFC");
            sb.AppendLine("into an XML file for property data and ");
            sb.AppendLine("a Wavefront OBJ file for 3D viewing");
            sb.AppendLine();
            sb.AppendLine("The model properties are linked");
            sb.AppendLine("to the 3D model using the IFC GUID");
            sb.AppendLine();
            sb.AppendLine("The first tab [IFC->Export obj file]");
            sb.AppendLine("shows the progress of exporting the");
            sb.AppendLine("Wavefront OBJ file");
            sb.AppendLine();
            sb.AppendLine("The second tab [IFC->Export xml file]");
            sb.AppendLine("shows the progress of exporting the xml");
            sb.AppendLine("properties file");
            sb.AppendLine();
            sb.AppendLine("The third tab [Viewer] Displays the");
            sb.AppendLine("IFC property tree and");
            sb.AppendLine("IFC models is a 3D viewer");
            sb.AppendLine();
            sb.AppendLine("tabs are auto magically selected");
            sb.AppendLine("as processes complete");
            sb.AppendLine();
            sb.AppendLine();

            this.textBox1.Text = sb.ToString();
        } 
    }
}
