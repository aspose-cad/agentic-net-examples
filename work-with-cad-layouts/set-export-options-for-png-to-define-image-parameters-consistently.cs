using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options for vector to raster conversion
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Define the size of the output image
                    PageWidth = 1600,
                    PageHeight = 1200,
                    // Optional: set background color, layout, layers, etc.
                    // BackgroundColor = System.Drawing.Color.White
                };

                // Set PNG export options and attach rasterization settings
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD drawing as a PNG image
                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine($"Export completed successfully: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error during processing: {ex.Message}");
        }
    }
}