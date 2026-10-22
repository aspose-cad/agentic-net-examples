using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Input CAD file (DWG, DXF, etc.)
            string inputPath = "sample.dwg";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare output directory and file paths
            string outputDir = "Output";
            Directory.CreateDirectory(outputDir);
            string objPath = Path.Combine(outputDir, "output.obj");
            string mtlPath = Path.Combine(outputDir, "output.mtl");

            // Load the source CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Set OBJ export options (default options will generate .mtl if materials exist)
                ObjOptions objOptions = new ObjOptions();

                // Save as OBJ (this will also generate the .mtl file if applicable)
                cadImage.Save(objPath, objOptions);
            }

            // Verify that both OBJ and MTL files were created
            if (!File.Exists(objPath))
            {
                Console.Error.WriteLine("Failed to create OBJ file.");
                return;
            }
            if (!File.Exists(mtlPath))
            {
                Console.Error.WriteLine("Failed to create MTL file.");
                return;
            }

            // Simple validation: attempt to open the OBJ file to ensure it is readable
            using (FileStream fs = File.OpenRead(objPath))
            {
                // If we can open the file without exception, consider it a pass
                Console.WriteLine("OBJ file opened successfully; validation passed.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}