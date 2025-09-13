// Ignore Spelling: Ifc ifctypes

using HeepWare.Data;
using System.Xml.Linq;

namespace HeepWare.Data.Project
{
    //IFC Element has attribute
    // ==> may have one or more of the reference links below
    //      property-set one more 
    //      quantity-set
    //      ifc type or style
    //      material
    //      materialLayerUsage
    //      presentationLayerAssignment
    //
    [System.Serializable]
    public class IFCProject
    {
        public List<IFCPropertySet> propertySets;
        public List<IfcElementQuantitySet> quantitySets;
        //public MaterialsCollection materialCollection;
        public List<IFCElement> IfcElements;
        public List<IFCType> ifctypes;
        public List<IFCPresentationLayer>? ifcPresentationLayers;

        public List<IFCRelConnectsPathElement> ifcRelConnectsPathElements;

        public List<IfcMaterial> materials = new List<IfcMaterial>();
        public List<IfcMaterialLayerSet> materialLayerSets = new List<IfcMaterialLayerSet>();
        public List<IfcMaterialList> materialLists = new List<IfcMaterialList>();
        public List<IfcMaterialLayerSetUsage> materialLayerSetUsages = new List<IfcMaterialLayerSetUsage>();
        public IFCProject()
        {
            propertySets = new List<IFCPropertySet>();
            quantitySets = new List<IfcElementQuantitySet>();
            //materialCollection = new MaterialsCollection();
            IfcElements = new List<IFCElement>();
            ifctypes = new List<IFCType>();
            ifcPresentationLayers = new List<IFCPresentationLayer>();

            ifcRelConnectsPathElements = new List<IFCRelConnectsPathElement>();
        }

        public string GetRootElementId()
        {
            string rootId = IfcElements.Where(x => x.ifcName == "IfcProject").Select(x => x.Id).First();
            return rootId;
        }


        public List<IFCPropertySet>? PropertySetForElementByElementId(string id)
        {
            IFCElement? elem = FindIFCElementById(id);
            if (elem != null)
            {
                string? propertyid = elem.links.Where(x => x.Key.EndsWith("PropertySet")).Select(x => x.Value).FirstOrDefault();
                if (propertyid != null)
                    return PropertySetById(propertyid);
            }
            return null;
        }

        public List<IFCPropertySet>? PropertySetById(string id)
        {
            id = id.Replace("#", "");
            if (propertySets != null)
            {
                return propertySets.Where(x => x.Id == id).ToList();
            }
            return null;
        }

        public List<IfcElementQuantitySet>? QuantitySetById(string id)
        {
            id = id.Replace("#", "");
            if (quantitySets != null) 
            {
                return quantitySets.Where(x => x.Id == id).ToList();
            }
            return null;
        }

        public List<IFCType>? TypeOrStyleById(string id)
        {
            id = id.Replace("#", "");
            if (ifctypes != null)
            {
                return ifctypes.Where(x => x.Id == id).ToList();
            }
            return null;
        }

        public List<string>? RelConnectsPathElementsById(string id)
        {
            List<string> connections = new List<string>();
            List<IFCRelConnectsPathElement> relPathConnections = ifcRelConnectsPathElements.Where(x => x.connectionLinks.Contains(id)).ToList();
            for (int i = 0; i < relPathConnections.Count; i++)
            {
                for (int n = 0; n < relPathConnections[i].connectionLinks.Count; n++)
                {
                    if (relPathConnections[i].connectionLinks[n] != id)
                    {
                        connections.Add(relPathConnections[i].connectionLinks[n]);
                    }
                }
            }
            return connections;
        }

        public List<IFCPresentationLayer>? PresentationLayerSetById(string id)
        {
            id = id.Replace("#", "");
            if (ifcPresentationLayers != null)
            {
                return ifcPresentationLayers.Where(x => x.Id == id).ToList();
            }
            return null;
        }

        public List<IfcMaterialLayerSetUsage>? MaterialLayerSetUsageById(string id)
        {
            id = id.Replace("#", "");
            if (materialLayerSetUsages != null)
            {
                return materialLayerSetUsages.Where(x => x.Id == id).ToList();
            }
            return null;
        }

        public List<IfcMaterialLayerSet>? MaterialLayerSetById(string id)
        {
            id = id.Replace("#", "");
            if (materialLayerSets != null)
            {
                return materialLayerSets.Where(x => x.Id == id).ToList();
            }
            return null;
        }

        public List<IfcMaterialList>? MaterialListById(string id)
        {
            id = id.Replace("#", "");
            if (materialLists != null)
            {
                return materialLists.Where(x => x.Id == id).ToList();
            }
            return null;
        }

        public List<IfcMaterial>? MaterialById(string id)
        {
            id = id.Replace("#", "");
            if (materials != null)
            {
                return materials.Where(x => x.Id == id).ToList();
            }
            return null;
        }

        public List<IFCType>? TypeById(string id)
        {
            id = id.Replace("#", "");
            if (ifctypes != null)
            {
                return ifctypes.Where(x => x.Id == id).ToList();
            }
            return null;
        }


        public List<IFCElement>? ElementGetParents(string elementId)
        {
            List<IFCElement> elements = new List<IFCElement>();
            IFCElement? ifcElement = FindIFCElementById(elementId);
            if (ifcElement != null)
            {
                elements.Add(ifcElement);
                while (ifcElement.parentId != null)
                {
                    ifcElement = FindIFCElementParentById(ifcElement.parentId);
                    if (ifcElement == null)
                        break;
                    elements.Add(ifcElement);
                }
            }
            return elements;
        }

        public List<IFCElement>? ElementsGivenParentId(string parentId)
        {
            if (this.IfcElements != null)
            {
                return this.IfcElements.Where(x => x.parentId == parentId).ToList();
            }
            return null;
        }

















        //public IFCElement? LoadElementWithChildrenIds(string id)
        //{
        //    IFCElement? element = this.IfcElements.Where(x => x.Id == id).FirstOrDefault();
        //    if (element != null)
        //    {
        //        //Load Children
        //        //element.children = ElementsGivenParentId(id);
        //        element.children = ElementsGivenParentId(id);
        //        if (element.children != null && element.children.Count > 0)
        //        {
        //            for (int i = 0; i < element.children.Count; i++)
        //            {
        //                IFCElement? element = 
        //                children[i].children = ElementsGivenParentId(element.children[i].Id);
        //            }
        //            children =
        //        }

        //    }
        //    return element;
        //}

        private IFCElement? LoadChildern(IFCElement elem)
        {
            elem.children = ElementsGivenParentId(elem.Id);

            return elem;
        }

        private IFCElement? WithChildern2(IFCElement? elem)
        {
            if (elem != null)
            {
                // Load related element connections if any exist
                elem.relPathConnections = RelConnectsPathElementsById(elem.Id);
                // Load this elements children
                elem = LoadChildern(elem);
                if (elem != null && elem.children != null && elem.children.Count > 0)
                {
                    for (int i = 0; i < elem.children.Count; i++)
                    {
                        elem.children[i].children = ElementsGivenParentId(elem.children[i].Id);
                        IFCElement? el = elem.children[i];
                        if (el != null)
                        {
                            el = WithChildern2(el);
                        }
                    }
                }
            }
            return elem;
        }

        public IFCElement? LoadElementWithChildren2(string id)
        {
            IFCElement? element = this.IfcElements.Where(x => x.Id == id).FirstOrDefault();
            if (element != null)
            {
                element = WithChildern2(element);
            }
            return element;
        }



















        public IFCElement? LoadElementWithDirectChildren(string id)
        {
            IFCElement? element = this.IfcElements.Where(x => x.Id == id).FirstOrDefault();
            if (element != null)
            {
                //Load Children
                element.children = ElementsGivenParentId(id);

                //load all related links to this element
                string? propertyid = element.links.Where(x => x.Key.EndsWith("PropertySet")).Select(x => x.Value).FirstOrDefault();
                if (propertyid != null)
                {
                    element.propertySets = PropertySetById(propertyid);
                }

                //load quantity links if any
                string? quantitySetId = element.links.Where(x => x.Key.EndsWith("QuantitySet")).Select(x => x.Value).FirstOrDefault();
                if (quantitySetId != null)
                {
                    element.quantitySets = QuantitySetById(quantitySetId);
                }

                //Load RelPathConnections
                List<string>? relConnections = RelConnectsPathElementsById(id);
                if (relConnections != null)
                {
                    if (element.relPathConnections == null)
                    {
                        element.relPathConnections = new List<string>();
                    }
                    element.relPathConnections.AddRange(relConnections);
                }

                //load type or style links if any
                string? typeOrStyleId = element.links.Where(x => x.Key.EndsWith("Type") || x.Key.EndsWith("Style")).Select(x => x.Value).FirstOrDefault();
                if (typeOrStyleId != null)
                {
                    element.ifctypes = TypeOrStyleById(typeOrStyleId);
                }

                //Load presentation layer
                string? presentationLayerAssignmentSetId = element.links.Where(x => x.Key.EndsWith("PresentationLayerAssignment")).Select(x => x.Value).FirstOrDefault();
                if (presentationLayerAssignmentSetId != null)
                {
                    element.presentationLayerSets = PresentationLayerSetById(presentationLayerAssignmentSetId);
                }

                //Load Material data 
                string? materialLayerSetUsageId = element.links.Where(x => x.Key.EndsWith("MaterialLayerSetUsage")).Select(x => x.Value).FirstOrDefault();
                if (materialLayerSetUsageId != null)
                {
                    element.materialLayerSetUsages = MaterialLayerSetUsageById(materialLayerSetUsageId);
                }

                string? materialLayerSetId = element.links.Where(x => x.Key.EndsWith("MaterialLayerSet")).Select(x => x.Value).FirstOrDefault();
                if (materialLayerSetId != null)
                {
                    element.materialLayerSets = MaterialLayerSetById(materialLayerSetId);
                }

                string? materialListId = element.links.Where(x => x.Key.EndsWith("MaterialList")).Select(x => x.Value).FirstOrDefault();
                if (materialListId != null)
                {
                    element.materialLists = MaterialListById(materialListId);
                }

                string? materialId = element.links.Where(x => x.Key.EndsWith("Material")).Select(x => x.Value).FirstOrDefault();
                if (materialId != null)
                {
                    element.materials = MaterialById(materialId);
                }

                if (element != null && element.children != null)
                {
                    for (int i = 0; i < element.children.Count; i++)
                    {
                        IFCElement? element1 = LoadElementWithDirectChildren(element.children[i].Id);
                        if (element1 != null)
                        {
                            element.children[i] = element1;
                        }
                    }
                }
            }
            return element;
        }



        private IFCElement? FindIFCElementById(string id)
        {
            IFCElement? elem = IfcElements.Where(x => x.Id == id).FirstOrDefault();
            if (elem != null)
            {
                Console.WriteLine("FindIFCElementById Id {0}, element name {1} element id {2}", id, elem.ifcName, elem.Id);
            }
            return elem;
        }

        private IFCElement? FindIFCElementParentById(string pid)
        {
            IFCElement? elem = IfcElements.Where(x => x.Id == pid).FirstOrDefault();
            if (elem != null)
            {
                Console.WriteLine("FindIFCElementParentById, parent id {0}, element name {1} element parent id {2}", pid, elem.ifcName, elem.parentId);
            }
            return elem;
        }
    }
}
