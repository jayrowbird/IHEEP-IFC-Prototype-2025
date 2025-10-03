using HeepWare.OBJ.IFC.Library;
using HeepWare.OBJ.Mesh.Data;
using System.Diagnostics;

namespace TestLoadOBJModel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = Stopwatch.StartNew();
            string filename = @"AssetImportData\f-16BySubmesh.obj"; 
            filename = @"AssetImportData\SimpleTable.obj";
            //filename = @"AssetImportData\flowers.obj";
            
            Console.WriteLine("Test Loading OBJ file {0}", filename);


            sw.Start();
            IFCOBJLib2 ifcOBJLib2 = new IFCOBJLib2(filename);

            ifcOBJLib2.LoadFile();

           List<MeshObject> meshes = ifcOBJLib2.GetMeshObjects();
            sw.Stop();
            Console.WriteLine("File loading took {0} Milliseconds", sw.ElapsedMilliseconds);
            Console.WriteLine("File loading took {0} seconds", sw.ElapsedMilliseconds/1000);

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
