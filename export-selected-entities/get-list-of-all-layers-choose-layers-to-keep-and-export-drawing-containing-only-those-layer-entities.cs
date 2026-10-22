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
        const string inputPath = "input.dwg";
        const string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing from file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD‑specific object model
                CadImage cad = (CadImage)cadImage;

                // Get the layer table containing all layer definitions
                var layerTable = cad.Layers;

                // List all layer names
                Console.WriteLine("Available layers:");
                foreach (CadLayerTable layerRecord in layerTable)
                {
                    Console.WriteLine("- " + layerRecord.Name);
                }

                // Define which layers should be kept in the exported image
                // Adjust the names to match layers present in your drawing
                string[] layersToKeep = new[] { "0", "Layer1" };

                // Configure rasterization options to include only the selected layers
                var rasterOptions = new CadRasterizationOptions
                {
                    Layers = layersToKeep,
                    PageWidth = 1600,   // optional: set output width
                    PageHeight = 1200,  // optional: set output height
                    Layouts = new[] { "Model" } // optional: specify layout(s)
                };

                // Choose PNG as the output format and assign the rasterization options
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the filtered drawing to a PNG file
                cad.Save(outputPath, pngOptions);
                Console.WriteLine($"Export completed: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}