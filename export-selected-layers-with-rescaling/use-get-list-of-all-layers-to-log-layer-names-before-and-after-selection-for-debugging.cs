using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        string inputPath = "sample.dwg";
        string outputPath = "output.png";

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

                // Log all layer names before any selection
                Console.WriteLine("Layers before selection:");
                foreach (CadLayerTable layerObj in cadImage.Layers)
                {
                    if (layerObj is CadLayerTable layer)
                    {
                        Console.WriteLine("- " + layer.Name);
                    }
                }

                // Define the layers we want to keep
                string[] selectedLayers = new[] { "0", "Layer1" };

                // Log the selected layers (after "selection")
                Console.WriteLine("Layers after selection:");
                foreach (string layerName in selectedLayers)
                {
                    Console.WriteLine("- " + layerName);
                }

                // Export the selected layers to PNG
                PngOptions pngOptions = new PngOptions();
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800,
                    PageHeight = 600,
                    Layers = selectedLayers
                };
                pngOptions.VectorRasterizationOptions = rasterOptions;

                cadImage.Save(outputPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}