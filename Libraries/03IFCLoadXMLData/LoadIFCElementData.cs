using System.Xml;

namespace HeepWare.Data.Load
{
    internal partial class LoadIFCElementData
    {
        private readonly bool DEBUGFLAG = false;
        private string mylevel = "";
        private string? pID = null; //parent id

        //below is for debugging only
        private List<string> elemtypes = new List<string>();
         
        internal List<IFCElement>  LoadIFCElements(XmlTextReader reader)
        {
            List<IFCElement> ifcElements = new List<IFCElement>();
            IFCElement? ifcElement = null;
            Stack<string?> st = new Stack<string?>(); // element parent id stack
            //Need the ifc element stack to load properties after child elements 
            Stack<IFCElement> elementStack = new Stack<IFCElement>(); // ifc element stack

            st.Push(null);

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:

                        string? nextId = null;
                        if (reader.IsEmptyElement)
                        {
                            //SelfClosing is either a child element or a link
                            //Child element creates a new element 
                            //link adds a link to the current element

                            if (ifcElement != null)
                            {
                                Dictionary<string, string> ifcAttributes = processSelfClosingAttributes(reader, out nextId);
                                string key = "xlink:href";
                                if (ifcAttributes.ContainsKey(key))
                                {
                                    //add this link to the existing IFCElement
                                    string value = ifcAttributes[key];
                                    string name = ifcAttributes["ifctype"];
                                    ifcElement.AddLink(name, value);
                                }
                                else
                                {
                                    ifcElement = new IFCElement();
                                    ifcElement.Id = ifcAttributes["id"];
                                    ifcElement.ifcName = ifcAttributes["ifctype"];
                                    ifcElement.parentId = st.First();
                                    string[] keys = ifcAttributes.Keys.ToArray();
                                    for (int n = 0; n < keys.Length; n++)
                                    {
                                        string value = ifcAttributes[keys[n]];
                                        ifcElement.AddAttribute(keys[n], value);
                                    }
                                    //ifcElements.Add(ifcElement);
                                    AddIFCElement(ifcElements, ifcElement);
                                }
                            }
                            //Set pId to the id of the child element just read
                            //if (nextId != null)
                            //{
                            //    //pID = nextId;
                            //    st.Push(nextId);
                            //}
                        }
                        else //this is an element with children
                        {
                            if (ifcElement != null)
                            {
                                //ifcElements.Add(ifcElement);
                                AddIFCElement(ifcElements, ifcElement);
                            }
                            ifcElement = LoadIFCElement(reader, out nextId);




                            elementStack.Push(ifcElement);
                            if (DEBUGFLAG) Console.WriteLine("Push {0} {1}", ifcElement.ifcName, ifcElement.Id);
                            if (true)
                            {
                                string? found = elemtypes.Where(x => x == ifcElement.ifcName).FirstOrDefault();
                                if (found == null)
                                {
                                    elemtypes.Add(ifcElement.ifcName);
                                }
                            }

                            if (nextId != null)
                            {
                                ifcElement.parentId = st.First();
                                //Set pId to the id of the child element just read
                                //pID = nextId;
                                st.Push(nextId);
                                //ifcElements.Add(ifcElement);
                                AddIFCElement(ifcElements, ifcElement);
                            }
                        }
                        break;


                    case XmlNodeType.EndElement:
                        st.Pop();

                        //ifcElements.Add(ifcElement);
                        if (ifcElement != null)
                        {
                            AddIFCElement(ifcElements, ifcElement);
                        }


                        if (elementStack.Count > 0)
                        {
                            IFCElement poppedElem = elementStack.Pop();

                            if (ifcElement != null)
                            {
                                if (DEBUGFLAG) Console.WriteLine("\tPop {0} {1}", ifcElement.ifcName, ifcElement.Id);
                            }

                            if (elementStack.Count > 0)
                                ifcElement = elementStack.First();
                        }


                        if (reader.Name == "decomposition")
                        {
                            if (true)
                            {
                                for (int n = 0; n < elemtypes.Count; n++)
                                {
                                    Console.WriteLine("{0}", elemtypes[n]);
                                }

                            }
                            return ifcElements;
                        }

                        //remove on tab
                        int i = mylevel.LastIndexOf('\t');
                        if (i >= 0)
                            mylevel = mylevel.Substring(0, i++);

                        if (DEBUGFLAG) Console.WriteLine("{0}END ELEMENT ++> {1}", mylevel, reader.Name);
                        break;
                }
            }
            return ifcElements;
        }

        private bool AddIFCElement(List<IFCElement> ifcElems, IFCElement elem)
        {
            bool result = false;

            IFCElement? found = ifcElems.Where(x => x.Id == elem.Id).FirstOrDefault();
            if (found == null)
            {
                ifcElems.Add(elem);
                result = true;
            }

            return result;
        }

        //private static List<KeyValuePair<string, string>> LoadIFCElementSelfClosing(XmlTextReader reader, out string? nId)
        //{ 
        //    List<KeyValuePair<string, string>> attribs = processSelfClosingAttributes(reader, out   nId);
        //    return attribs;
        //}

        private IFCElement LoadIFCElement(XmlTextReader reader, out string? nId)
        {
            //save the current element name so we can test with it to know when we are done processing th node
            IFCElement ifcElement = new IFCElement();

            ifcElement.ifcName = reader.Name;
            Dictionary<string, string> attrib = ProcessElementAttributes(reader, out nId);
            if (attrib != null)
            {
                string[] keys = attrib.Keys.ToArray();
                for (int n = 0; n < keys.Length; n++)
                {
                    if (keys[n] == "id")
                    {
                        ifcElement.Id = attrib[keys[n]];
                    }
                    string value = attrib[keys[n]];
                    ifcElement.AddAttribute(keys[n], value);
                }
            }
            return ifcElement;
        }

        private Dictionary<string, string> processSelfClosingAttributes(XmlTextReader reader, out string? id)
        {
            //Parent id to the next child element
            id = null;
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            attributes.Add("ifctype", reader.Name);
            reader.MoveToFirstAttribute();
            for (int i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);

                if (reader.Name == "id")
                    id = reader.Value;

                attributes.Add(reader.Name, reader.Value);
            }
            return attributes;
        }

        private Dictionary<string, string> ProcessElementAttributes(XmlTextReader reader, out string? id)
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
