using System.Xml;

namespace HeepWare.Data.Load
{
    internal partial class LoadIFCElementData
    { 
        internal MaterialsCollection? LoadMaterials(XmlTextReader reader)
        {
            MaterialsCollection materialsCollection = new MaterialsCollection();

            IfcMaterial? material = null;
            IfcMaterialList? materialList = null;
            IfcMaterialLayer? materialLayer = null;
            IfcMaterialLayerSetUsage? materialLayerSetUsage = null;
            IfcMaterialLayerSet? materialLayerSet = null;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.EndElement:

                        if (reader.Name == "materials")
                            return materialsCollection;
                        //remove on tab
                        int i = mylevel.LastIndexOf('\t');
                        if (i >= 0)
                            mylevel = mylevel.Substring(0, i++);

                        if (DEBUGFLAG) Console.WriteLine("{0}END ELEMENT ++> {1}", mylevel, reader.Name);
                        break;

                    case XmlNodeType.Element:

                        //self closing element switch
                        bool selfclosing = false;
                        string value = "";
                        if (reader.HasValue)
                        {
                            value = reader.Value.Trim();
                        }
                        //if (reader.IsEmptyElement)
                        //{
                        //    material = new IfcMaterial();
                        //    if (reader.HasAttributes)
                        //    {
                        //        typeAttributes(reader, material);
                        //        materialsCollection.Add(material);
                        //    }
                        //}

                        if (reader.IsStartElement())
                        {
                            if (reader.Name == "IfcMaterialList")
                            {

                                string? Id = "";
                                materialList = new IfcMaterialList();
                                materialList.attributes = ProcessMaterialAttributes(reader, out Id);
                                if (Id != null)
                                {
                                    materialList.Id = Id;
                                    while (reader.NodeType != XmlNodeType.EndElement)
                                    {
                                        reader.Read();
                                        material = new IfcMaterial();
                                        if (reader.HasAttributes)
                                        {
                                            typeAttributes(reader, material);
                                            materialList.Add(material);
                                        }
                                    }
                                    if (reader.NodeType == XmlNodeType.EndElement)
                                    {
                                        materialsCollection.Add(materialList);
                                    }
                                }
                            }
                            else if (reader.Name == "IfcMaterialLayerSet")
                            {
                                string? Id = "";
                                materialLayerSet = new IfcMaterialLayerSet();
                                materialLayerSet.attributes = ProcessMaterialAttributes(reader, out Id);
                                if (Id != null)
                                {
                                    materialLayerSet.Id = Id;
                                    while (reader.NodeType != XmlNodeType.EndElement)
                                    {
                                        reader.Read();
                                        materialLayer = new IfcMaterialLayer();
                                        if (reader.HasAttributes)
                                        {
                                            materialLayer.attributes = ProcessMaterialAttributes(reader, out Id);

                                            materialLayerSet.Add(materialLayer);
                                        }
                                    }
                                    if (reader.NodeType == XmlNodeType.EndElement)
                                    {
                                        materialsCollection.Add(materialLayerSet);
                                    }
                                }
                            }
                            else if (reader.Name == "IfcMaterialLayerSetUsage")
                            {
                                string? Id = "";
                                materialLayerSetUsage = new IfcMaterialLayerSetUsage();
                                materialLayerSetUsage.attributes = ProcessMaterialAttributes(reader, out Id);
                                if (Id != null)
                                {
                                    materialLayerSetUsage.Id = Id;
                                    while (reader.NodeType != XmlNodeType.EndElement)
                                    {
                                        reader.Read();
                                        materialLayer = new IfcMaterialLayer();
                                        if (reader.HasAttributes)
                                        {
                                            materialLayer.attributes = ProcessMaterialAttributes(reader, out Id);

                                            materialLayerSetUsage.Add(materialLayer);
                                        }
                                    }
                                    if (reader.NodeType == XmlNodeType.EndElement)
                                    {
                                        materialsCollection.Add(materialLayerSetUsage);
                                    }
                                }
                            }
                            else if (reader.Name == "IfcMaterial")
                            {
                                //reader.Read();
                                material = new IfcMaterial();
                                if (reader.HasAttributes)
                                {
                                    typeAttributes(reader, material);
                                    materialsCollection.Add(material);
                                }
                            }
                        }
                        break;

                    
                }
            }
            return null;
        }
        private void typeAttributes(XmlTextReader reader, IfcMaterial ifcm)
        {
            reader.MoveToFirstAttribute();

            for (int i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);
                if (reader.Name == "id") { ifcm.Id = reader.Value; }
                else if(reader.Name == "Name") { ifcm.Name = reader.Value; }
                else if (reader.Name == "Category") { ifcm.Category = reader.Value; }
                else
                {
                    Console.WriteLine("\tIfcMaterial has unknown value {0}", reader.Value);
                }
                if (DEBUGFLAG) Console.WriteLine("\t{0}Attribute ==> {1}, {2}", mylevel, reader.Name, reader.Value);
            }
        }

        private Dictionary<string, string> ProcessMaterialAttributes(XmlTextReader reader, out string? id)
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
