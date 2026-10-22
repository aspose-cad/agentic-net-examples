using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class ConfigurePenOptionsExample
{
    static void Main()
    {
        try
        {
            // Input CAD file path
            string sourcePath = "sample.dwg";

            // Verify the source file exists
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            // Load the CAD image
            using (CadImage cadImage = (CadImage)Image.Load(sourcePath))
            {
                // Create rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,
                    LineScale = 1.0f
                };

                // Configure PenOptions
                PenOptions penOptions = new PenOptions
                {
                    StartCap = LineCap.Flat,
                    EndCap = LineCap.Flat
                };

                // NOTE: The Aspose.CAD SDK does not expose a DashStyle or DashPattern property
                // on PenOptions, so a dotted dash style cannot be set via the API.
                // This limitation is reported explicitly instead of providing a non‑existent member.

                // Assign PenOptions to rasterization options
                rasterOptions.PenOptions = penOptions;

                // Prepare PNG output options and attach rasterization settings
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Ensure output directory exists
                string outputDir = "output";
                Directory.CreateDirectory(outputDir);
                string outputPath = Path.Combine(outputDir, "output.png");

                // Render and save the CAD drawing as a PNG image
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Image saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}