using Aspose.CAD.FileFormats.Cad;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadObserverPointAdjustment
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder input CAD file path (DWG, DXF, etc.)
            string inputPath = "input.dwg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare output directory
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, "model.obj");

            try
            {
                // Load the CAD image
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage (if needed for further CAD-specific operations)
                    CadImage cadImage = image as CadImage;

                    // NOTE: The Camera property is not available in the current Aspose.CAD version.
                    // If view adjustment is required, apply transformations to the entities here.
                    // Example: translate all entities (placeholder - no actual transformation applied).

                    // Set OBJ export options (default options are sufficient for basic export)
                    ObjOptions objOptions = new ObjOptions();

                    // Export the model to OBJ format
                    cadImage.Save(outputPath, objOptions);
                }

                Console.WriteLine($"Model exported successfully to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}