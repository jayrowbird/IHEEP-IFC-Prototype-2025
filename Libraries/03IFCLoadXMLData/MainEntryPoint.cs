using HeepWare.Data.Load;
using HeepWare.Data.Project;
using System.Diagnostics;
using System.Xml;

namespace HeepWare.Data.XML.Load.EntryPoint
{
    public class MainEntryPoint
    {
        private string filename;
        private const bool DEBUGFLAG = false;

        private Stopwatch stopWatch = new Stopwatch();
        private IFCProject ifcProject = new IFCProject();

        private LoadIFCElementData loadIFCElementData;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public MainEntryPoint(string filePath)
        {
            filename = filePath;

            loadIFCElementData = new LoadIFCElementData();

            LoadIFCData();
        }

        public IFCProject GetIFCProject()
        {
            return ifcProject;
        }

        private void LoadIFCData()
        {
            XmlTextReader? reader = null;
            try
            {
                stopWatch.Start();
                // Load the reader with the data file and ignore all white space nodes.
                reader = new XmlTextReader(filename);
                reader.WhitespaceHandling = WhitespaceHandling.None;
                // reader.MoveToContent();
                // Parse the file and display each of the nodes.

                while (reader.Read())
                {
                    Console.WriteLine("reader.Name {0}", reader.Name);
                    if(reader.Name.Contains("groups") == true)
                    {
                        string t = "";
                    }
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name == "connections")
                            {
                                if (reader.IsEmptyElement == true) continue;
                                ifcProject.ifcRelConnectsPathElements = loadIFCElementData.LoadIFCRelConnections(reader);
                            }
                            if (reader.Name == "properties")
                            {
                                if (reader.IsEmptyElement == true) continue;
                                ifcProject.propertySets = loadIFCElementData.LoadProperties(reader);
                            }
                            if (reader.Name == "quantities")
                            {
                                if (reader.IsEmptyElement == false && reader.AttributeCount > 0)
                                {
                                    ifcProject.quantitySets = loadIFCElementData.LoadQuantities(reader);
                                }
                            }
                            if (reader.Name == "types")
                            {
                                if (reader.IsEmptyElement == true) continue;
                                ifcProject.ifctypes = loadIFCElementData.LoadTypes(reader);
                            }
                            if (reader.Name == "layers")
                            {
                                if(reader.IsEmptyElement == true) continue;

                                ifcProject.ifcPresentationLayers = loadIFCElementData.LoadPresentationLayers(reader);
                            }
                            if (reader.Name == "materials")
                            {
                                //materials section contains IfcMaterialLayerSetUsage or IfcMaterialList or IfcMaterial

                                MaterialsCollection? materialsCollection = loadIFCElementData.LoadMaterials(reader);
                                if (materialsCollection != null)
                                {
                                    ifcProject.materials = materialsCollection.materials;
                                    ifcProject.materialLayerSets = materialsCollection.materialLayerSets;
                                    ifcProject.materialLists = materialsCollection.materialLists;
                                    ifcProject.materialLayerSetUsages = materialsCollection.materialLayerSetUsages;
                                }
                                materialsCollection = null; //garbage collect materialsCollection

                            }
                            if (reader.Name == "decomposition")
                            {
                                ifcProject.IfcElements = loadIFCElementData.LoadIFCElements(reader);
                            }
                            // self closing element switch
                            bool selfclosing = false;
                            string value = "";
                            if (reader.HasValue)
                            {
                                value = reader.Value.Trim();
                            }

                            if (reader.IsEmptyElement)
                            {
                                selfclosing = true;
                            }

                            // if (DEBUGFLAG) Console.WriteLine("{0}Element ==>  {1}, {2} ", mylevel, reader.Name.Trim(), value);
                            if (reader.HasAttributes)
                            {
                                ProcessAttributes(reader);
                            }
                            break;
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            stopWatch.Stop();
            long milliseconds = stopWatch.ElapsedMilliseconds;
            Console.WriteLine("Completed parsing xml file :: {0} millisec", milliseconds);
            
            if (ifcProject != null && ifcProject.IfcElements != null)
            {
                Console.WriteLine("Number of IFC elements found: {0}", ifcProject.IfcElements.Count);
            }
        }

        /// <summary>
        /// load element attribute data 
        /// </summary>
        /// <param name="reader"></param>
        private void ProcessAttributes(XmlTextReader reader)
        {
            reader.MoveToFirstAttribute();
            for (int i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);
                // if (DEBUGFLAG) Console.WriteLine("\t{0}Attribute ==> {1}, {2}", mylevel, reader.Name, reader.Value);
            }
        }
    }
}
