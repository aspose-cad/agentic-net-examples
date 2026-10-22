using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

namespace AsposeCadObjInspection
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the OBJ file (the corresponding .mtl file should be in the same folder)
                string objFilePath = "sample.obj";

                if (!File.Exists(objFilePath))
                {
                    Console.WriteLine($"OBJ file not found: {objFilePath}");
                    return;
                }

                // Load the OBJ file using Aspose.CAD to ensure it is a supported CAD format
                using (Image image = Image.Load(objFilePath))
                {
                    Image cadImage = image ;
                    if (cadImage == null)
                    {
                        Console.WriteLine("The loaded file is not a supported CAD image.");
                        return;
                    }

                    // ------------------------------------------------------------
                    // Check for presence of vertex normals in the OBJ file
                    // ------------------------------------------------------------
                    bool normalsPresent = File.ReadLines(objFilePath)
                                             .Any(line => line.StartsWith("vn ", StringComparison.OrdinalIgnoreCase));

                    Console.WriteLine("Vertex normals present in OBJ: " + normalsPresent);

                    // ------------------------------------------------------------
                    // Verify that material properties are defined in the associated .mtl file
                    // ------------------------------------------------------------
                    bool materialsDefined = false;
                    string mtlFileName = null;

                    // Look for the mtllib directive inside the OBJ file
                    foreach (var line in File.ReadLines(objFilePath))
                    {
                        if (line.StartsWith("mtllib ", StringComparison.OrdinalIgnoreCase))
                        {
                            mtlFileName = line.Substring(7).Trim();
                            break;
                        }
                    }

                    if (!string.IsNullOrEmpty(mtlFileName))
                    {
                        string mtlFilePath = Path.Combine(Path.GetDirectoryName(objFilePath) ?? string.Empty, mtlFileName);
                        if (File.Exists(mtlFilePath))
                        {
                            // Simple check: at least one material definition (newmtl) exists
                            materialsDefined = File.ReadLines(mtlFilePath)
                                                   .Any(l => l.StartsWith("newmtl ", StringComparison.OrdinalIgnoreCase));
                        }
                    }

                    Console.WriteLine("Material definitions present in .mtl: " + materialsDefined);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}