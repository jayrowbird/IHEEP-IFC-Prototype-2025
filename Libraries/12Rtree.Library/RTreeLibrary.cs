using HeepWare.OBJ.Data;
using HeepWare.OBJ.Mesh.Data;
using RTree;

namespace HeepWare.RTree.Library
{
    public class RTreeLibrary
    {
        private RTree<string>? rtree = null;
        private List<MeshObject> connectedMeshes = new List<MeshObject>();
        private List<string> names = new List<string>();
        private List<MeshObject> modelObjects = new List<MeshObject>();
        private List<Connection> connections = new List<Connection>();

        private void ClearRTree()
        {
            if(rtree != null) {rtree = null;}
            connectedMeshes.Clear();
            names.Clear();
            modelObjects.Clear();
            connections.Clear();
        }

        public void Initialize ()
        {
            InitializeRTree();
        }

        public int GetRTreeCount()
        {
            if (rtree != null)
            { 
            return rtree.Count;
        }
            return -1;
        }
        private void InitializeRTree()
        {
            ClearRTree();
            rtree = new RTree<string>();
        }


        /// <summary>
        /// Load wavefront mesh objects into the rtree
        /// </summary>
        /// <param name="meshObjects"></param>
        /// <exception cref="Exception"></exception>
        public void LoadMeshObjects(List<MeshObject> meshObjects)
        {

            if (meshObjects != null && rtree != null)
            {
                modelObjects.AddRange(meshObjects);
                //load the mesh object bounding boxes into the rtree
                for (int i = 0; i < meshObjects.Count; i++)
                {
                    BBox bbox = meshObjects[i].GetBBox();
                    //rtree.Add(new Rectangle(bbox.GetMin(), bbox.GetMax()), meshObjects[i].GetName());
                    //-------------------------------------------------------------------------------------
                    // Adding 0.01 to all values in the mesh objects bbox to make up for drawing errors
                    float[] min = bbox.GetMin();
                    min[0] -= 0.01f; min[1] -= 0.01f; min[2] -= 0.01f;
                    float[] max = bbox.GetMax();
                    max[0] += 0.01f; max[1] += 0.01f; max[2] += 0.01f;

                    rtree.Add(new Rectangle(min, max), meshObjects[i].GetName());
                    //-------------------------------------------------------------------------------------
                }
            }
            else 
            {
                if (rtree == null)
                {
                    throw new Exception("rtree is null");
                }
                else
                {
                    throw new Exception("meshObjects is null");
                }
            }
        }

        /// <summary>
        /// pass in the mesh object to start the search
        /// </summary>
        /// <param name="meshObject"></param>
        public List<Connection> Search(MeshObject meshObject)
        {
            if (meshObject != null && rtree != null)
            {
                BBox bBox = meshObject.GetBBox();
                names = rtree.Intersects(new Rectangle(bBox.GetMin(), bBox.GetMax()));

                if (names != null)
                {
                    for (int i = 0; i < names.Count; i++)
                    {
                        MakeNewSearchBasedOnResults(names[i],modelObjects);
                    }
                }
            }
            else
            {
                if (rtree == null)
                {
                    throw new Exception("rtree is null");
                }
                else
                {
                    throw new Exception("meshObjects is null");
                }
            }

            return connections;
        }

        /// <summary>
        /// Use the found intersected mesh BBoxes to continue the search for connected meshes
        /// </summary>
        /// <param name="name"></param>
        /// <param name="meshObjects"></param>
        /// <returns></returns>
        private bool MakeNewSearchBasedOnResults(string name, List<MeshObject> meshObjects)
        {
            if (rtree == null)
            {
                throw new Exception("rtree is null");
            }
            bool result = AddFoundMesh(name);
            if (result)
            {
                MeshObject? meshObject1 = meshObjects.Where(x => x.GetName() == (name)).FirstOrDefault();
                if (meshObject1 != null)
                {
                    BBox bBox1 = meshObject1.GetBBox();
                    List<string> names1 = rtree.Intersects(new Rectangle(bBox1.GetMin(), bBox1.GetMax()));
                    AddNamesUsedToSearch(names1);

                    //----------------------------------------------------------------
                    Connection connection = new Connection();
                    connection.meshName = meshObject1.GetName();
                    connection.AddRange(names1);
                    //if(meshObject1.GetName() == "p10.5")
                    //{
                    //    string t = "";
                    //}
                    connections.Add(connection);
                    //----------------------------------------------------------------
                }
            }
            return result;
        }
        /// <summary>
        /// Make sure the found mesh object is only added to the list once and only once
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool AddFoundMesh(string name)
        {
            if (name == null) return false;
            bool result = false;
            MeshObject? storedMesh = connectedMeshes.Where(x => x.GetName() == name).FirstOrDefault();
            if (storedMesh == null)
            {
                storedMesh = modelObjects.Where(x => x.GetName() == name).FirstOrDefault();
                if (storedMesh != null)
                {
                    connectedMeshes.Add(storedMesh);
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_names_"></param>
        private void AddNamesUsedToSearch(List<string> _names_)
        {
            if (_names_ != null)
            {
                for (int i = 0; i < _names_.Count; i++)
                {
                    string? existingName = names.Where(x => x == _names_[i]).FirstOrDefault();
                    if (existingName == null)
                    {
                        names.Add(_names_[i]);
                    }
                }
            }
        }
    }
}
