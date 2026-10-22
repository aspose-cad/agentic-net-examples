using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "sample.dwg";
        string outputDir = "output";

        try
        {
            // Verify input file exists
            if (!File.Exists(inputPath))
                throw new FileNotFoundException("Input file not found.", inputPath);

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Load CAD image
            using (Image cadImage = Image.Load(inputPath))
            {
                // Base rasterization options
                var rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Millimeter,
                    PageWidth = 1000,
                    PageHeight = 1000,
                    AutomaticLayoutsScaling = true,
                    LineScale = 1.0f // default line scale
                };

                // Apply predefined quality settings
                ConfigureQuality(rasterOptions, lineScale: 2.0f);

                // PNG export options
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                string outputPath = Path.Combine(outputDir, "output.png");
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Image saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // Configures quality-related settings for CadRasterizationOptions.
    // Aspose.CAD does not expose TextQuality, HatchQuality, or ArcQuality.
    // Rendering fidelity can be improved by adjusting LineScale.
    static void ConfigureQuality(CadRasterizationOptions options, float lineScale)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));
        options.LineScale = lineScale;
        // Future quality properties can be set here when supported by the API.
    }
}