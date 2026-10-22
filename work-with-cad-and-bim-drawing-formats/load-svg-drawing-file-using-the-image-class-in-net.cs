using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
                // Path to the source SVG file
                string inputPath = "input.svg";

        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Path to the output PNG file
                string outputPath = "output/output.png";

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the SVG drawing using Aspose.CAD.Image
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to work with CAD-specific features
                    CadImage cadImage = (CadImage)image;

                    // Iterate over block entities using the typed collection
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        // Example: display block name and number of entities it contains
                        Console.WriteLine($"Block: {block.Name}, Entities: {block.Entities.Count}");
                    }

                    // Configure rasterization options for PNG export
                    var rasterOptions = new CadRasterizationOptions
                    {
                        UnitType = UnitType.Centimenter, // SDK's actual spelling
                        LineScale = 1.0f
                    };

                    // Set up PNG export options
                    var pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the image as PNG
                    image.Save(outputPath, pngOptions);
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}