using System.Xml;

namespace HeepWare.Data.Load
{
    internal partial class LoadIFCElementData
    {
        internal List<IFCType>  LoadTypes(XmlTextReader reader)
        {
            List<IFCType> ifcTypes = new List<IFCType>();
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
                        IFCType ifctype = new IFCType();
                        if (reader.HasAttributes)
                        {
                            typeAttributes(reader, ifctype);
                            ifcTypes.Add(ifctype);
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
                        if (reader.Name == "types")
                            return ifcTypes;

                        //remove on tab
                        int i = mylevel.LastIndexOf('\t');
                        if (i >= 0)
                            mylevel = mylevel.Substring(0, i++);

                        if (DEBUGFLAG) Console.WriteLine("{0}END ELEMENT ++> {1}", mylevel, reader.Name);
                        break;
                }
            }

            return ifcTypes;
        }
        private void typeAttributes(XmlTextReader reader, IFCType ifctp)
        {
            reader.MoveToFirstAttribute();

            for (int i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);
                if (reader.Name == "id") { ifctp.Id = reader.Value; }
                else
                {
                    ifctp.Add(reader.Name, reader.Value);
                }

                if (DEBUGFLAG) Console.WriteLine("\t{0}Attribute ==> {1}, {2}", mylevel, reader.Name, reader.Value);
            }
        }
    }
}
