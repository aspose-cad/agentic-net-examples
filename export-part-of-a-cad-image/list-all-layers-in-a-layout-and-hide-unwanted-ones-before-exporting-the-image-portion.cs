using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source CAD file
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                var cad = (CadImage)cadImage;

                // List all layer names in the drawing
                Console.WriteLine("Layers in the drawing:");
                foreach (CadLayerTable layer in cad.Layers)
                {
                    Console.WriteLine("- " + layer.Name);
                }

                // Define which layers should remain visible (others will be hidden)
                List<string> layersToKeep = new List<string> { "0", "VisibleLayer" };

                // Configure rasterization options: select a layout and the layers to render
                var rasterOptions = new CadRasterizationOptions();

                // Choose the first layout if layouts are present
                string firstLayoutName = cad.Layouts.Keys.FirstOrDefault();
                if (!string.IsNullOrEmpty(firstLayoutName))
                {
                    rasterOptions.Layouts = new[] { firstLayoutName };
                }

                // Apply the layer filter
                rasterOptions.Layers = layersToKeep.ToArray();

                // Set up PNG export options and attach rasterization settings
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the filtered image
                string outputPath = "output.png";
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Exported image saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}