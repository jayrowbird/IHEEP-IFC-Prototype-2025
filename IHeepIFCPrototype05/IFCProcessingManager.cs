using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace HeepWare.Windows.Forms.IFC.Prototype
{
    internal class IFCProcessingManager
    {
        private System.Diagnostics.Process? objProcess = null;
        private System.Diagnostics.Process? xmlProcess = null;

        private readonly string IFCconvert = ".\\IFCconvert\\ifcConvert.exe";
        private string conversionOutputPath = @".\ConversionOutput";
        private string OBJconvertStr = "-v --use-element-guids {FULLFILENAME}.ifc .\\ConversionOutput\\{FILENAME}.obj";
        private string XMLconvertStr = "-v --use-element-guids {FULLFILENAME}.ifc .\\ConversionOutput\\{FILENAME}.xml";
        private string selectedFilename = string.Empty;

        public string IFCfullFilename;
        public string IFCobjFullFilename;
        public string IFCxmlFullFilename;
        //public bool IFCconversion2objSuccessful;
        //public bool IFCconversion2xmlSuccessful;
        private TextBox objConversionText;
        private TextBox xmlConversionText;
 
        private bool destroyed = true;

        public IFCProcessingManager(TextBox objFileText, TextBox xmlFileText)
        {
            objConversionText = objFileText;
            xmlConversionText = xmlFileText;
            destroyed = true;
        }

        public bool SetIFCFullFilename(string name)
        {
            IFCfullFilename = name;
            return true;
        }

        public void DisplayIFCModel()
        {

        }

        

        public Process Convert2OBJ( )
        {
            
           return RunConverter2OBJ(IFCfullFilename );
        }

        private Process RunConverter2OBJ(string fullfilename )
        {
            if (objProcess != null)
            {
                objProcess.Close();
                objProcess.Dispose();
                objProcess = null;
            }

            objConversionText.Text = ""; 

            string argsOBJconvertStr = OBJconvertStr.Replace("{FULLFILENAME}", fullfilename);
            string filename = Path.GetFileNameWithoutExtension(fullfilename);
            IFCobjFullFilename = string.Format("{0}\\{1}.{2}", conversionOutputPath, filename,"obj");
            argsOBJconvertStr = argsOBJconvertStr.Replace("{FILENAME}", filename);

            //Check for existing file ?if found rename it with a date extension
            string existingFilename = ".\\ConversionOutput\\{FILENAME}.obj".Replace("{FILENAME}", filename);
            if (File.Exists(existingFilename) == true)
            {
                //SHow the user an existing filename of the same name was found, it was renamed with the date appended
                string newFilename = existingFilename + ("_" + System.DateTime.Now.ToShortDateString() + "_" + System.DateTime.Now.ToShortTimeString()).Replace("/", "_").Replace(" ", "").Replace(":", "_");
                System.IO.File.Move(existingFilename, newFilename);

                objConversionText.Text += new string('_', 65) + Environment.NewLine;
                objConversionText.Text += string.Format("Found existing file =>'{0}' renaming it to file named =>'{1}'{2}", existingFilename, newFilename, Environment.NewLine);
                objConversionText.Text += new string('_', 65) + Environment.NewLine;
                objConversionText.Text += Environment.NewLine;
            }


            objConversionText.Text += "Executing the following command" + Environment.NewLine;
            objConversionText.Text += string.Format("{0} {1} {2}{3}", IFCconvert, argsOBJconvertStr, Environment.NewLine, Environment.NewLine);

            objProcess = new System.Diagnostics.Process();
            objProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //process.StartInfo.FileName =  "cmd.exe";
            objProcess.StartInfo.FileName = IFCconvert;
            objProcess.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            //process.StartInfo.Arguments = @"/C DIR C:\";
            objProcess.StartInfo.Arguments = argsOBJconvertStr;
            objProcess.StartInfo.UseShellExecute = false;
            objProcess.StartInfo.CreateNoWindow = false;
            objProcess.StartInfo.RedirectStandardOutput = true;
            objProcess.StartInfo.RedirectStandardError = true;
            objProcess.StartInfo.CreateNoWindow = true;
            objProcess.EnableRaisingEvents = true;
            // see below for output handler
            objProcess.ErrorDataReceived += proc_DataReceived1;
            objProcess.OutputDataReceived += proc_DataReceived1;

            xmlConversionText.Text = $"c:/ hi mom  {Environment.NewLine}"; 
             
            objProcess.Start();

            //process.BeginErrorReadLine();
            objProcess.BeginOutputReadLine(); 
             

            //process.WaitForExit();
            return objProcess;
        }

         
        public Process Convert2XML()
        {
          return  RunConverter2XML(IFCfullFilename);
        }

        private Process RunConverter2XML(string fullfilename)
        {
            if (xmlProcess != null)
            {
                xmlProcess.Close();
                xmlProcess.Dispose();
                xmlProcess = null;
            }

            xmlConversionText.Text = "";

            string argsXMLconvertStr = XMLconvertStr.Replace("{FULLFILENAME}", fullfilename);
            string filename = Path.GetFileNameWithoutExtension(fullfilename);
            IFCxmlFullFilename = string.Format("{0}\\{1}.{2}", conversionOutputPath, filename, "xml");
            argsXMLconvertStr = argsXMLconvertStr.Replace("{FILENAME}", filename);

            //Check for existing file ?if found rename it with a date extension
            string existingFilename = ".\\ConversionOutput\\{FILENAME}.xml".Replace("{FILENAME}", filename);
            if (File.Exists(existingFilename) == true)
            {
                //SHow the user an existing filename of the same name was found, it was renamed with the date appended

                string newFilename = existingFilename + ("_" + System.DateTime.Now.ToShortDateString() + "_" + System.DateTime.Now.ToShortTimeString()).Replace("/", "_").Replace(" ", "").Replace(":", "_");
                System.IO.File.Move(existingFilename, newFilename);

                xmlConversionText.Text += new string('_', 65) + Environment.NewLine;
                xmlConversionText.Text += string.Format("Found existing file =>'{0}' renaming it to file named =>'{1}'{2}", existingFilename, newFilename, Environment.NewLine);
                xmlConversionText.Text += new string('_', 65) + Environment.NewLine;
                xmlConversionText.Text += Environment.NewLine;
            }


            xmlConversionText.Text += "Executing the following command" + Environment.NewLine;
            xmlConversionText.Text += string.Format("{0} {1} {2}{3}", IFCconvert, argsXMLconvertStr, Environment.NewLine, Environment.NewLine);

            xmlProcess = new System.Diagnostics.Process();
            xmlProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            xmlProcess.StartInfo.FileName = IFCconvert;
            xmlProcess.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            xmlProcess.StartInfo.Arguments = argsXMLconvertStr;
            xmlProcess.StartInfo.UseShellExecute = false;
            xmlProcess.StartInfo.CreateNoWindow = false;
            xmlProcess.StartInfo.RedirectStandardOutput = true;
            xmlProcess.StartInfo.RedirectStandardError = true;
            xmlProcess.EnableRaisingEvents = true;
            xmlProcess.StartInfo.CreateNoWindow = true;
            // see below for output handler
            xmlProcess.ErrorDataReceived += proc_DataReceived2;
            xmlProcess.OutputDataReceived += proc_DataReceived2;
            xmlProcess.Start();

            xmlProcess.BeginErrorReadLine();
            xmlProcess.BeginOutputReadLine();

            return xmlProcess;
        }

        void proc_DataReceived1(object sender, DataReceivedEventArgs e)
        {
            Process myProcess = sender as Process;
            if (myProcess == null)
                return;


            objConversionText.Invoke(new Action(delegate
            {
                if (e != null && e.Data != null)
                {
                    string text = e.Data.Replace("\0", "").Replace("\r", "").Replace("\n", "");
                    if (!string.IsNullOrEmpty(text))
                    {
                        //if(text.StartsWith("#") )
                        //{

                        //}
                        objConversionText.AppendText(text + Environment.NewLine);

                    }

                }
            })
            );
        }


        void proc_DataReceived2(object sender, DataReceivedEventArgs e)
        {
            Process myProcess = sender as Process;
            if (myProcess == null)
                return;


            xmlConversionText.Invoke(new Action(delegate
            {
                if (e != null && e.Data != null)
                {
                    string text = e.Data.Replace("\0", "").Replace("\r", "").Replace("\n", "");
                    if (!string.IsNullOrEmpty(text))
                    {
                        //if(text.StartsWith("#") )
                        //{

                        //}
                        xmlConversionText.AppendText(text + Environment.NewLine); 
                    }

                }
            })
            );
        }

        public bool IsDisposed()
        {
            return destroyed;
        }

        public bool Dispose()
        {
            if (objProcess != null)
            {
                objProcess.Close();
                objProcess.Dispose();
                objProcess = null;
            }
            if (xmlProcess != null)
            {
                xmlProcess.Close();
                xmlProcess.Dispose();
                xmlProcess = null;
            }
            destroyed = true;
            return destroyed;
        }



    }
}
