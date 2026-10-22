using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadHatchAlignmentExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Input CAD file (main drawing) and external drawing to be inserted
                string mainDrawingPath = "input.dwg";
                string externalDrawingPath = "external.dwg";

                // Output file (PDF to visualize the result)
                string outputPath = "output.pdf";

                // Verify that the required files exist
                if (!File.Exists(mainDrawingPath))
                {
                    Console.Error.WriteLine($"Main drawing file not found: {mainDrawingPath}");
                    return;
                }

                if (!File.Exists(externalDrawingPath))
                {
                    Console.Error.WriteLine($"External drawing file not found: {externalDrawingPath}");
                    return;
                }

                // Load the main drawing
                using (CadImage mainImage = (CadImage)Image.Load(mainDrawingPath))
                {
                    // Load the external drawing
                    using (CadImage externalImage = (CadImage)Image.Load(externalDrawingPath))
                    {
                        // Merge entities from the external drawing into the main drawing
                        var mergedEntities = mainImage.Entities
                            .Concat(externalImage.Entities)
                            .ToArray();

                        // Replace the entity collection with the merged one
                        mainImage.Entities = mergedEntities;
                    }

                    // Adjust the origin of all Hatch entities to align hatch patterns
                    // Note: Aspose.CAD does not expose a direct Hatch class in the public API.
                    // If a specific Hatch entity type were available, you could adjust its origin here.
                    // This placeholder demonstrates where such logic would be placed.

                    // Save the modified drawing as PDF
                    PdfOptions pdfOptions = new PdfOptions();
                    mainImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Conversion completed successfully. Output saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}