using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace AsposeCadExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Input and output paths
                string inputPath = "input.dwg";
                string outputDir = "output";
                string outputPath = Path.Combine(outputDir, "output.dwg");

                // Verify input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Ensure output directory exists
                Directory.CreateDirectory(outputDir);

                // Load the CAD drawing
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Apply a default font for all TEXT objects
                    cadImage.DefaultFont = "Arial";

                    // Example: modify individual TEXT entities if needed
                    foreach (var textEntity in cadImage.Entities.OfType<CadText>())
                    {
                        // You can set properties like TextHeight, TextRotation, etc., here
                        // For this example we keep defaults
                    }

                    // Arrange layout: access the first layout (if any)
                    var firstLayout = cadImage.Layouts.Values.FirstOrDefault();
                    if (firstLayout != null)
                    {
                        // Layout properties are read‑only in the current API version,
                        // so we cannot rename it directly. Additional layout adjustments
                        // would be performed here if supported.
                    }

                    // Save the modified drawing as DWG
                    var dwgOptions = new DwgOptions();
                    cadImage.Save(outputPath, dwgOptions);
                }

                Console.WriteLine($"Drawing saved successfully to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}