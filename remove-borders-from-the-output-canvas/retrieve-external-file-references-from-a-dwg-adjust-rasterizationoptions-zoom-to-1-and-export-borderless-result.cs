using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

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
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Attempt to list external references via block entities (if any)
                foreach (var block in cadImage.BlockEntities.Values)
                {
                    // Placeholder: external reference handling not directly supported in current API
                }

                var rasterizationOptions = new CadRasterizationOptions
                {
                    Zoom = 1
                    // DrawBorder property is unavailable; output will be borderless by default
                };

                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterizationOptions
                };

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