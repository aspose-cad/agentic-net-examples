using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cadImage = (CadImage)image;

                // Collect names of all visible layers (ColorId >= 0 and not frozen)
                List<string> visibleLayerNames = new List<string>();
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    bool isOff = layer.ColorId < 0;
                    bool isFrozen = (layer.Flags & 1) != 0; // Bit 0 indicates frozen state
                    if (!isOff && !isFrozen)
                    {
                        visibleLayerNames.Add(layer.Name);
                    }
                }

                // Configure rasterization options to include only the visible layers
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layers = visibleLayerNames.ToArray()
                };

                // Set up PDF export options (PDF/A compliance can be set if supported)
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                    // If PDF/A compliance is required and the property exists, uncomment the following line:
                    // Compliance = PdfCompliance.PdfA1b
                };

                // Export the drawing to PDF, ensuring all visible layers are present
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Export completed successfully: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}