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
            Console.WriteLine($"Input file \"{inputPath}\" not found.");
            return;
        }

        try
        {
            // Load the CAD file
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Collect names of visible layers (ColorId >= 0 and not frozen)
                List<string> visibleLayerNames = new List<string>();
                foreach (CadLayerTable obj in cadImage.Layers)
                {
                    if (obj is CadLayerTable layer)
                    {
                        bool isOff = layer.ColorId < 0;
                        bool isFrozen = (layer.Flags & 1) != 0;
                        if (!isOff && !isFrozen)
                        {
                            visibleLayerNames.Add(layer.Name);
                        }
                    }
                }

                // If there are no visible layers, skip export
                if (visibleLayerNames.Count == 0)
                {
                    Console.WriteLine("No visible layers to export.");
                    return;
                }

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new[] { "Model" },
                    Layers = visibleLayerNames.ToArray()
                };

                // Set PDF save options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD drawing as PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Export completed successfully to \"{outputPath}\".");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}