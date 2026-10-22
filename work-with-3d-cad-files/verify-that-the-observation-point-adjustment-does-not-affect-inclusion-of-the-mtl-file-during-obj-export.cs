using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadObjExportCheck
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define input CAD file path (placeholder)
                string inputPath = "sample.dwg";

                // Verify input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Prepare output directory
                string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
                Directory.CreateDirectory(outputDir);

                // Define output OBJ file path
                string objPath = Path.Combine(outputDir, "exported.obj");

                // Load CAD image
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Set up rasterization options (without ObservationPoint)
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();

                    // Configure OBJ export options
                    ObjOptions exportOptions = new ObjOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Export to OBJ format
                    cadImage.Save(objPath, exportOptions);
                }

                // Verify that the associated .mtl file was generated
                string mtlPath = Path.ChangeExtension(objPath, ".mtl");
                if (File.Exists(mtlPath))
                {
                    Console.WriteLine($".mtl file successfully generated at: {mtlPath}");
                }
                else
                {
                    Console.Error.WriteLine($".mtl file was not generated. Expected at: {mtlPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}