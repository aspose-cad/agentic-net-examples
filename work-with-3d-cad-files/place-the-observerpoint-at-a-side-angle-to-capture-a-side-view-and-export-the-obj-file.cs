using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file (DWG, DXF, etc.)
                string sourcePath = "input.dwg";

                // Path for the exported OBJ file
                string outputPath = "output.obj";

                // Verify that the source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Load the CAD file
                using (Image cadImage = Image.Load(sourcePath))
                {
                    // Create OBJ export options (default view is used)
                    ObjOptions objOptions = new ObjOptions();

                    // Export the CAD model to OBJ format
                    cadImage.Save(outputPath, objOptions);
                }

                Console.WriteLine("OBJ file has been exported successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}