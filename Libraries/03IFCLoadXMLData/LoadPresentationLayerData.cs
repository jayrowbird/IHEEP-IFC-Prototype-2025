using HeepWare.Data;
using System.Xml;

namespace HeepWare.Data.Load
{
    internal partial class LoadIFCElementData
    {
        internal List<IFCPresentationLayer>? LoadPresentationLayers(XmlTextReader reader)
        {
            List<IFCPresentationLayer> ifcPresentationLayers = new List<IFCPresentationLayer>();
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:

                        //self closing element switch
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
                        IFCPresentationLayer ifcPresentationLayer = new IFCPresentationLayer();
                        if (reader.HasAttributes)
                        {
                            typeAttributes(reader, ifcPresentationLayer);
                            ifcPresentationLayers.Add(ifcPresentationLayer);
                        }

                        if (DEBUGFLAG) Console.WriteLine("{0}Element ==>  {1}, {2} ", mylevel, reader.Name.Trim(), value);

                        if (reader.IsStartElement() && selfclosing == false)
                        {
                            //add one tab
                            mylevel += "\t";
                        }
                        break;

                    case XmlNodeType.Text:
                        Console.WriteLine("{0} {1}", mylevel, reader.Value);
                        break;

                    case XmlNodeType.EndElement:
                        if (reader.Name == "layers")
                            return ifcPresentationLayers;

                        //remove on tab
                        int i = mylevel.LastIndexOf('\t');
                        if (i >= 0)
                            mylevel = mylevel.Substring(0, i++);

                        if (DEBUGFLAG) Console.WriteLine("{0}END ELEMENT ++> {1}", mylevel, reader.Name);
                        break;
                }
            }

            return null;
        }
        private void typeAttributes(XmlTextReader reader, IFCPresentationLayer ifcPL)
        {
            reader.MoveToFirstAttribute();

            for (int i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);
                if (reader.Name == "id") { ifcPL.Id = reader.Value; }
                else
                {
                    ifcPL.Name = reader.Value;
                }

                if (DEBUGFLAG) Console.WriteLine("\t{0}Attribute ==> {1}, {2}", mylevel, reader.Name, reader.Value);
            }
        }
    }
}
