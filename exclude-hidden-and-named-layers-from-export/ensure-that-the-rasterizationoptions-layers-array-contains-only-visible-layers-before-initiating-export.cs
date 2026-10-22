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
            Console.WriteLine($"Input file \"{inputPath}\" not found. Export aborted.");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Determine visible layers (ColorId >= 0 and not frozen)
                List<string> visibleLayerNames = new List<string>();
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    bool isOff = layer.ColorId < 0;
                    bool isFrozen = (layer.Flags & 1) != 0;
                    if (!isOff && !isFrozen)
                    {
                        visibleLayerNames.Add(layer.Name);
                    }
                }

                if (visibleLayerNames.Count == 0)
                {
                    Console.WriteLine("No visible layers found. Export aborted.");
                    return;
                }

                // Configure rasterization options to include only visible layers
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layers = visibleLayerNames.ToArray()
                };

                // Set up PDF save options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Export the drawing to PDF using the filtered layers
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