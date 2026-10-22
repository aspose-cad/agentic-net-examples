using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class ConfigurePenOptionsExample
{
    static void Main()
    {
        string inputPath = "input.dxf";
        string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD file.
            CadImage cadImage = (CadImage)Image.Load(inputPath);

            // Configure rasterization options.
            // Note: PenOptions does not provide a property to set pen width directly.
            // Line thickness can be influenced via LineScale.
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                UnitType = UnitType.Centimenter,
                LineScale = 2.0f,
                PenOptions = new PenOptions
                {
                    StartCap = LineCap.Round,
                    EndCap = LineCap.Round
                }
            };

            // Ensure the output directory exists.
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Set PNG output options.
            PngOptions pngOptions = new PngOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save the rasterized image.
            cadImage.Save(outputPath, pngOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}