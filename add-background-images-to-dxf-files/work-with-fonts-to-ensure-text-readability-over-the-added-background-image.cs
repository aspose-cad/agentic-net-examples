using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Input CAD file and output image path
            string cadPath = "input.dwg";
            string finalOutputPath = "output.png";

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(finalOutputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Verify CAD file exists
            if (!File.Exists(cadPath))
            {
                Console.Error.WriteLine($"CAD file not found: {cadPath}");
                return;
            }

            // Load CAD drawing
            using (Image cadImage = Image.Load(cadPath))
            {
                // Set rasterization options with transparent background
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = cadImage.Size.Width,
                    PageHeight = cadImage.Size.Height,
                    BackgroundColor = Aspose.CAD.Color.Transparent
                };

                // Configure PNG output options
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Render CAD to PNG
                cadImage.Save(finalOutputPath, pngOptions);
            }

            Console.WriteLine($"Output image saved to: {finalOutputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
