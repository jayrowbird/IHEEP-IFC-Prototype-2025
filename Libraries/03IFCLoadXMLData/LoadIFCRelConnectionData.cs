using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Xml;

namespace HeepWare.Data.Load
{
    internal partial class LoadIFCElementData
    {

        internal List<IFCRelConnectsPathElement> LoadIFCRelConnections(XmlTextReader reader)
        {
            List<IFCRelConnectsPathElement> ifcConnections = new List<IFCRelConnectsPathElement>();
            IFCRelConnectsPathElement ifcConnection;
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.IsStartElement())
                        {
                            if (reader.Name == "IfcRelConnectsPathElements")
                            {
                                string? Id = "";
                                ifcConnection = new IFCRelConnectsPathElement();
                                ifcConnection.attributes = ProcessAttributes(reader, out Id);

                                if (Id != null)
                                {
                                    ifcConnection.Id = Id;
                                    while (reader.Read())
                                    {
                                        if (reader.HasAttributes == true)
                                        {
                                            ifcConnection!.AddLink(processSelfClosingAttributes(reader));
                                        }
                                        else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "IfcRelConnectsPathElements")
                                        {
                                            if (ifcConnection != null)
                                            {
                                                ifcConnections.Add(ifcConnection);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;


                    case XmlNodeType.EndElement:
                        if (reader.Name == "connections")
                        {
                            return ifcConnections;
                        }
                        else
                        {
                            if (DEBUGFLAG) Console.WriteLine("END ELEMENT ++> {0}", reader.Name);
                        }
                        break;
                }
            }
            return ifcConnections;
        }

        private string processSelfClosingAttributes(XmlTextReader reader)
        {
            string link = string.Empty;

            reader.MoveToFirstAttribute();
            for (int i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);
                link = reader.Value.Replace("#", "");
                break;
            }
            return link;
        }
        private Dictionary<string, string> ProcessAttributes(XmlTextReader reader, out string? id)
        {
            id = null;
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            reader.MoveToFirstAttribute();
            for (int i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);
                if (reader.Name == "id")
                {
                    id = reader.Value;
                }

                attributes.Add(reader.Name, reader.Value);
            }
            return attributes;
        }
    }
}
