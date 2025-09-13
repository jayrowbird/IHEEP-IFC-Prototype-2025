using OpenTK.Mathematics;

namespace HeepWare.RTree.Library
{ 
    public struct Triangle
    {
        internal Vector3 p1;
        internal Vector3 p2;
        internal Vector3 p3;

        public Vector3 GetP1()
        {
            return p1;
        }

        public Vector3 GetP2()
        {
            return p2;
        }

        public Vector3 GetP3()
        {
            return p3;
        }
        public Triangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            p1 = v1;
            p2 = v2;
            p3 = v3;
        }
    };
}
