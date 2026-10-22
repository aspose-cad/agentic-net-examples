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
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                List<string> visibleLayers = new List<string>();

                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    bool isOff = layer.ColorId < 0;
                    bool isFrozen = (layer.Flags & 1) != 0;
                    bool isVisible = !isOff && !isFrozen;

                    Console.WriteLine($"Layer: {layer.Name}, Visible: {isVisible}");

                    if (isVisible)
                    {
                        visibleLayers.Add(layer.Name);
                    }
                }

                if (visibleLayers.Count == 0)
                {
                    Console.WriteLine("No visible layers to export.");
                    return;
                }

                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layers = visibleLayers.ToArray()
                };

                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Exported PDF saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}