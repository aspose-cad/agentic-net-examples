using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadBlockLineWeightExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file (DWG, DXF, etc.)
                string sourcePath = "input.dwg";

                // Verify that the source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Path for the exported file
                string outputPath = "output.pdf";

                // Load the CAD drawing
                using (Image image = Image.Load(sourcePath))
                {
                    // Cast the loaded image to CadImage to access CAD object model
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("Failed to load CAD image.");
                        return;
                    }

                    // Iterate through all block definitions in the drawing
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        // Iterate through each entity inside the block
                        foreach (var entity in block.Entities)
                        {
                            // Change line weight for entities that support it
                            // LineWeight expects a short value (in hundredths of a millimeter)
                            entity.LineWeight = (short)50; // 0.5 mm
                        }
                    }

                    // Export the modified drawing to PDF
                    PdfOptions pdfOptions = new PdfOptions();
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine("Drawing exported successfully to " + outputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}