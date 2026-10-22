using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Input CAD file path (replace with your actual file)
        string inputPath = @"C:\Input\sample.dwg";
        // Output directory
        string outputDir = @"C:\Output";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD image
            Image cadImage = Image.Load(inputPath);

            // Rendering with default PenOptions (width implicitly zero/default)
            CadRasterizationOptions rasterOptionsZero = new CadRasterizationOptions
            {
                PageWidth = 1600,
                PageHeight = 1200,
                PenOptions = new PenOptions() // default settings
            };

            PngOptions pngOptionsZero = new PngOptions
            {
                VectorRasterizationOptions = rasterOptionsZero
            };

            string outputPathZero = Path.Combine(outputDir, "render_default_width.png");
            cadImage.Save(outputPathZero, pngOptionsZero);
            Console.WriteLine($"Saved image with default PenOptions to: {outputPathZero}");

            // Rendering with PenOptions where we modify a different property (e.g., color) to illustrate change
            CadRasterizationOptions rasterOptionsModified = new CadRasterizationOptions
            {
                PageWidth = 1600,
                PageHeight = 1200,
                PenOptions = new PenOptions() // still default width, but we can set other properties if needed
            };

            // Example of setting another property (optional)
            // rasterOptionsModified.PenOptions.Color = System.Drawing.Color.Red;

            PngOptions pngOptionsModified = new PngOptions
            {
                VectorRasterizationOptions = rasterOptionsModified
            };

            string outputPathModified = Path.Combine(outputDir, "render_modified_penoptions.png");
            cadImage.Save(outputPathModified, pngOptionsModified);
            Console.WriteLine($"Saved image with modified PenOptions to: {outputPathModified}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}