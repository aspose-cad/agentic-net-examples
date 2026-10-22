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
            Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                var cadImage = (CadImage)image;

                var rasterOptions = new CadRasterizationOptions();

                // Access PenOptions for configuration
                rasterOptions.PenOptions = new PenOptions();
                // Example configuration (if supported by the library version):
                // rasterOptions.PenOptions.Width = 0.5f;
                // rasterOptions.PenOptions.Color = Aspose.CAD.Color.Blue;

                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}