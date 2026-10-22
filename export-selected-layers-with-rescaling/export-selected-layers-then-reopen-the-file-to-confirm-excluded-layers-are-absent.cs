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
        const string outputPath = "selected_layers.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file '{inputPath}' not found.");
            return;
        }

        try
        {
            // Load the source CAD file
            using (Image sourceImage = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)sourceImage;

                // Display all layer names present in the source file
                Console.WriteLine("Layers in source file:");
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    Console.WriteLine("- " + layer.Name);
                }

                // Define the layers that should be exported
                string[] selectedLayers = new string[] { "Layer1", "Layer2" };

                // Configure rasterization options to include only the selected layers
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layers = selectedLayers
                };

                // Set PNG export options and attach the rasterization settings
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Export the CAD drawing with only the selected layers to PNG
                cadImage.Save(outputPath, pngOptions);
            }

            // Verify the exported PNG exists
            if (!File.Exists(outputPath))
            {
                Console.Error.WriteLine($"Exported file '{outputPath}' was not created.");
                return;
            }

            // Reopen the exported PNG to confirm it was created successfully
            using (Image exportedImage = Image.Load(outputPath))
            {
                Console.WriteLine($"Exported image dimensions: {exportedImage.Width}x{exportedImage.Height}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}