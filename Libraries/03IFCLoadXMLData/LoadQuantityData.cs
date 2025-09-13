using System.Xml;

namespace HeepWare.Data.Load
{
    internal partial class LoadIFCElementData
    {
        internal List<IfcElementQuantitySet> LoadQuantities(XmlTextReader reader)
        {
            List<IfcElementQuantitySet> quantitySets = new List<IfcElementQuantitySet>();
            IfcElementQuantitySet quantitySet = new IfcElementQuantitySet();
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

                        if (reader.Name == "IfcElementQuantity")
                        {
                            quantitySet = new IfcElementQuantitySet();
                            if (reader.HasAttributes)
                            {
                                propertySetAttributes(reader, quantitySet);
                                quantitySets.Add(quantitySet);
                            }
                        }
                        else
                        {
                            if (quantitySet != null)
                                propertyAttributes(reader, quantitySet);
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
                        if (reader.Name == "quantities")
                            return quantitySets;

                        //remove on tab
                        int i = mylevel.LastIndexOf('\t');
                        if (i >= 0)
                            mylevel = mylevel.Substring(0, i++);

                        if (DEBUGFLAG) Console.WriteLine("{0}END ELEMENT ++> {1}", mylevel, reader.Name);
                        break;
                }
            }

            return new List<IfcElementQuantitySet>();
        }
        private void propertySetAttributes(XmlTextReader reader, IfcElementQuantitySet pSet)
        {
            reader.MoveToFirstAttribute();
            for (int i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);
                if (reader.Name == "id")
                {
                    pSet.Id = reader.Value;
                }
                else if (reader.Name == "Name")
                {
                    pSet.Name = reader.Value;
                }
                if (DEBUGFLAG) Console.WriteLine("\t{0}Attribute ==> {1}, {2}", mylevel, reader.Name, reader.Value);
            }
        }
        private void propertyAttributes(XmlTextReader reader, IfcElementQuantitySet pSet)
        {
            reader.MoveToFirstAttribute();
            IfcQuantity property = new IfcQuantity(); ;
            for (int i = 0; i < reader.AttributeCount; i++)
            {

                reader.MoveToAttribute(i);
                if (i % 2 == 0)
                {
                    property = new IfcQuantity();
                    property.Name = reader.Value;
                    if (reader.AttributeCount == 1)
                    {
                        pSet.Add(property);
                    }
                }
                else
                {
                    property.Value = reader.Value;
                    pSet.Add(property);
                }
                if (DEBUGFLAG) Console.WriteLine("\t{0}Attribute ==> {1}, {2}", mylevel, reader.Name, reader.Value);
            }
        }
    }
}
