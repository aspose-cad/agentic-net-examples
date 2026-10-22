using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Path to the source DWF file.
        string sourcePath = "input.dwf";
        // Path where the BMP file will be saved.
        string outputPath = "output.bmp";

        // Verify that the source file exists.
        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        // Ensure the output directory exists.
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the DWF drawing.
            using (Image dwfImage = Image.Load(sourcePath))
            {
                // Set up default rasterization options.
                var rasterOptions = new CadRasterizationOptions();

                // Create BMP save options and assign the rasterization options.
                var bmpOptions = new BmpOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the drawing as a BMP image using the default conversion process.
                dwfImage.Save(outputPath, bmpOptions);
            }

            Console.WriteLine($"Conversion completed successfully. BMP saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}