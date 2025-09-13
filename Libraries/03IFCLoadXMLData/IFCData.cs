// Ignore Spelling: Ifc ifctypes

namespace HeepWare.Data
{
    [System.Serializable]
    public class IFCElement
    {
        public string? parentId = null;
        public string Id = "";
        public string ifcName = "";
        public Dictionary<string, string> attributes;
        public Dictionary<string, string> links;

        public List<IFCPropertySet>? propertySets = null;
        public List<IfcElementQuantitySet>? quantitySets = null;

        public List<string>? relPathConnections = null;

        public List<IFCType>? ifctypes = null; 

        public List<IFCPresentationLayer>? presentationLayerSets = null;

        public List<IfcMaterialLayerSetUsage>? materialLayerSetUsages = null;
        public List<IfcMaterialLayerSet>? materialLayerSets = null;
        public List<IfcMaterialList>? materialLists = null;
        public List<IfcMaterial>? materials = null;
        internal List<MaterialsCollection>? materialsCollections = null;

        public List<IFCElement>? children = null;

        public IFCElement()
        {
            attributes = new Dictionary<string, string>();
            links = new Dictionary<string, string>();

        }
        public void AddAttribute(string key, string value)
        {
            attributes.Add(key, value);
        }
        public void AddLink(string key, string value)
        {
            int i = 1;
            string newkey = key;
            //append unique int to the from of the duplicate link name
            while (links.ContainsKey(newkey) == true)
            {
                newkey = string.Format("{0}{1}", i++, key);
            }
            links.Add(newkey, value);
        }
    }

    [System.Serializable]
    public class IFCRelConnectsPathElement
    {
        public string Id = "";
        public string name = "";
        public Dictionary<string, string> attributes;
        public List< string> connectionLinks;

        public IFCRelConnectsPathElement()
        {
            attributes = new Dictionary<string, string>();
            connectionLinks = new List<string>();
        }

        public void Add(string key, string value)
        {
            attributes.Add(key, value);
        }
        public void AddLink(string id)
        {
            connectionLinks.Add(id);
        }
    }

    [System.Serializable]
    public class IFCType
    {
        public string Id = "";
        public Dictionary<string, string> attributes;

        public IFCType()
        {
            attributes = new Dictionary<string, string>();
        }

        public void Add(string key, string value)
        {
            attributes.Add(key, value);
        }
    }

    [System.Serializable]
    public class IFCPresentationLayer
    {
        public string Id = "";
        public string Name = "";

        public IFCPresentationLayer()
        {
        }
    }




    [System.Serializable]
    public class IFCProperty
    {
        public string Name = "";
        public string Value = "";
    }

    [System.Serializable]
    public class IFCPropertySet
    {
        public string Name = "";
        public string Id = "";

        public Dictionary<string, string> properties;
        public IFCPropertySet()
        {
            properties = new Dictionary<string, string>();
        }

        public void Add(IFCProperty p)
        {
            properties.Add(p.Name, p.Value);
        }
    }

    [System.Serializable]
    public class IfcElementQuantitySet
    {
        public string Name = "";
        public string Id = "";

        public Dictionary<string, string> properties;
        public IfcElementQuantitySet()
        {
            properties = new Dictionary<string, string>();
        }
        public void Add(IfcQuantity p)
        {
            properties.Add(p.Name, p.Value);
        }

    }

    [System.Serializable]
    public class IfcQuantity
    {
        public string Name = "";
        public string Value = "";
    }

    /// <summary>
    /// Material Layer Usage set contains a list of material layers
    /// </summary>
    [System.Serializable]
    public class IfcMaterialLayerSetUsage
    {

        public string Id = "";
        public Dictionary<string, string> attributes;

        public List<IfcMaterialLayer> materialLayers;

        public IfcMaterialLayerSetUsage()
        {
            materialLayers = new List<IfcMaterialLayer>();
            attributes = new Dictionary<string, string>();
        }

        public void Add(IfcMaterialLayer p)
        {
            materialLayers.Add(p);
        }
    }

    /// <summary>
    /// Material Layer set contains a list of material layers
    /// </summary>
    [System.Serializable]
    public class IfcMaterialLayerSet 
    {

        public string Id = "";
        public Dictionary<string, string> attributes;

        public List<IfcMaterialLayer> materialLayers;

        public IfcMaterialLayerSet ()
        {
            materialLayers = new List<IfcMaterialLayer>();
            attributes = new Dictionary<string, string>();
        }

        public void Add(IfcMaterialLayer p)
        {
            materialLayers.Add(p);
        }
    }

    [System.Serializable]
    public class IfcMaterialLayer
    {
        public Dictionary<string, string> attributes;
        public IfcMaterialLayer()
        {
            attributes = new Dictionary<string, string>();
        }

    }

    [System.Serializable]
    public class IfcMaterialList
    {
        public string Id = "";
        public Dictionary<string, string> materials;
        public Dictionary<string, string> attributes;

        public IfcMaterialList()
        {
            materials = new Dictionary<string, string>();
            attributes = new Dictionary<string, string>();
        }
        public void Add(IfcMaterial m)
        {
            materials.TryAdd(m.Name, m.Id);
        }
    }
    [System.Serializable]
    public class MaterialsCollection
    {
        public List<IfcMaterial> materials = new List<IfcMaterial>();
        public List<IfcMaterialLayerSet> materialLayerSets = new List<IfcMaterialLayerSet>();
        public List<IfcMaterialList> materialLists = new List<IfcMaterialList>();
        public List<IfcMaterialLayerSetUsage> materialLayerSetUsages = new List<IfcMaterialLayerSetUsage>();

        internal void Add(IfcMaterial m)
        {
            materials.Add((IfcMaterial)m);
        }

        internal void Add(IfcMaterialLayerSet m)
        {
            materialLayerSets.Add((IfcMaterialLayerSet)m);
        }

        internal void Add(IfcMaterialList m)
        {
            materialLists.Add((IfcMaterialList)m);
        }

        internal void Add(IfcMaterialLayerSetUsage m)
        {
            materialLayerSetUsages.Add((IfcMaterialLayerSetUsage)m);
        }
    }

    [System.Serializable]
    public class IfcMaterial
    {
        public string Id = "";
        public string Name = "";
        public string Category = "";
    }
}
