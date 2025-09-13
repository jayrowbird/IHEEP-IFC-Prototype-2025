// Ignore Spelling: IFC

using HeepWare.Data;
using HeepWare.Data.Project;
using HeepWare.Data.XML.Load.EntryPoint;
using Newtonsoft.Json;

namespace HeepWare.IFC.XML.LoadData.Test
{
    internal class Program
    {
        private readonly bool DEBUGFLAG = false;
        private const String filename = "TestData\\HeadQuarters.xml";
        static int totalelems = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            string filename1 = "TestData\\Building-Architecture.xml";
            List<string> filenames = new List<string>();

            filenames.Add("TestData\\10034108-A-CENTRAL_V2018.xml");
            filenames.Add("TestData\\Building-Architecture.xml");
            filenames.Add("TestData\\infra-Plumbing.xml");
            filenames.Add("TestData\\Building-Hvac.xml");
            filenames.Add("TestData\\HeadQuarters.xml");
            filenames.Add("TestData\\infra-Rail.xml");
            filenames.Add("TestData\\infra-Road.xml");
            filenames.Add("TestData\\HeadQuarters.xml");

            for (int i = 0; i < filenames.Count; i++)
            {
                Console.WriteLine("Parsing file {0}", filenames[i]);
                new Program().Run(filenames[i]);
            }
            Console.WriteLine("Done made it to the end");
            return;

            MainEntryPoint mainEntryPoint = new MainEntryPoint(filename1);

            IFCProject ifcProject = mainEntryPoint.GetIFCProject();

            string rootId = ifcProject.GetRootElementId();

            if (false)
            {
                List<string> ids = new List<string>();
                for (int i = 0; i < ifcProject.IfcElements.Count; i++)
                {
                    ids.Add(ifcProject.IfcElements[i].Id);
                }
                ids.Sort();
                for (int i = 0; i < ids.Count; i++)
                {
                    Console.WriteLine("{0}", ids[i]);
                }
            }



            //IFCElement? elem_all = ifcProject.LoadElementWithChildrenIds("0DNWI2k6L6CglTxC23nIIJ");


            //sb this is huge please test

            if (false)
            {
                IFCElement? elem_all_building = ifcProject.LoadElementWithChildren2("0DNWI2k6L6CglTxC23nIIJ");
                PrintChildernNames(elem_all_building);

                string output1 = JsonConvert.SerializeObject(elem_all_building);
                File.WriteAllText("ExampleAllIFCJson.json", output1);
            }


            IFCElement? elem_all_arch = ifcProject.LoadElementWithChildren2(rootId);
            PrintChildernNames(elem_all_arch);


            //Make json string
            string output = JsonConvert.SerializeObject(elem_all_arch);
            File.WriteAllText("ExampleArchIFCJson.json", output);




            //ifcProject.ElementTypeGetParents("38uwnxCqvDav40wL0T5YKH");  
            List<IFCElement> results = ifcProject.ElementsGivenParentId("0DNWI2k6L6CglTxC1yCBeA");

            List<IFCPropertySet>? results1 = ifcProject.PropertySetForElementByElementId("1vgaZ41uj0fAutvgFaVbFi");

            IFCElement element = ifcProject.LoadElementWithDirectChildren("0DNWI2k6L6CglTxC1yCBbr");

            IFCElement element1 = ifcProject.LoadElementWithDirectChildren("3zjfQeojz5yeK5yyJkPpsh");

            IFCElement element2 = ifcProject.LoadElementWithDirectChildren("38uwnxCqvDav40wL0T5XhQ");

            Console.WriteLine("Done");
            Console.ReadKey();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        private void Run(string filename)
        {
            MainEntryPoint mainEntryPoint = new MainEntryPoint(filename);

            IFCProject ifcProject = mainEntryPoint.GetIFCProject();

            //test connections
            if (ifcProject.ifcRelConnectsPathElements != null && ifcProject.ifcRelConnectsPathElements.Count > 0)
            {
                List<IFCRelConnectsPathElement> connections = ifcProject.ifcRelConnectsPathElements;
                string id = connections[0].connectionLinks[0];

                List<IFCRelConnectsPathElement> withid = connections.Where(x => x.connectionLinks.Contains(id)).ToList();

                  id = connections[0].connectionLinks[1];

                 withid = connections.Where(x => x.connectionLinks.Contains(id)).ToList();
            }
            //

            string rootId = ifcProject.GetRootElementId();

            IFCElement? elem_all_data = ifcProject.LoadElementWithChildren2(rootId);
            if (elem_all_data != null)
            {
                PrintChildernNames(elem_all_data);
            }

            string file = Path.GetFileNameWithoutExtension(filename);

            //Make json string
            string output = JsonConvert.SerializeObject(elem_all_data);
            File.WriteAllText(string.Format("{0}IFCJson.json", file), output);
            Console.WriteLine("Done with {0}", file);
        }

        private static void PrintChildernNames(IFCElement elem)
        {
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

    }
}
