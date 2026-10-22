using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main(string[] args)
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

            // Path for the output image without borders
            string outputPath = "output.png";

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Unable to load the CAD file as a CadImage.");
                    return;
                }

                // Retrieve and display the list of layers
                if (cadImage.Layers != null)
                {
                    Console.WriteLine("Layers in the drawing:");
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        Console.WriteLine($"- {layer.Name}");
                    }
                }

                // Configure rasterization options with zero margins to eliminate canvas borders
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Margins = new Margins { Left = 0, Right = 0, Top = 0, Bottom = 0 },
                    UnitType = UnitType.Centimenter,
                    LineScale = 1.0f,
                    // Ensure no extra margins when zoom is 1
                    Zoom = 1
                };

                // Set up PNG output options and assign the rasterization settings
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the drawing to PNG without any surrounding borders
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Image saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}