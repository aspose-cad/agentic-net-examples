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
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Build a list of layer names to include, excluding "LayerToHide"
                List<string> visibleLayers = new List<string>();
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    // Exclude the specific layer by name
                    if (string.Equals(layer.Name, "LayerToHide", StringComparison.OrdinalIgnoreCase))
                        continue;

                    // Determine if the layer is off (ColorId < 0) or frozen (Flags bit 0 set)
                    bool isOff = layer.ColorId < 0;
                    bool isFrozen = (layer.Flags & 1) != 0;

                    if (!isOff && !isFrozen)
                        visibleLayers.Add(layer.Name);
                }

                // If no layers are left to export, exit
                if (visibleLayers.Count == 0)
                {
                    Console.WriteLine("No visible layers to export.");
                    return;
                }

                // Configure rasterization options to include only the selected layers
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layers = visibleLayers.ToArray()
                };

                // Set up PDF export options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the filtered drawing to PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Export completed: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}