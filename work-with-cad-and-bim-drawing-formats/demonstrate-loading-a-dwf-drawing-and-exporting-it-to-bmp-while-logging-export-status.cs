using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Placeholder paths – replace with actual file locations.
        string inputPath = @"C:\Input\sample.dwf";
        string outputPath = @"C:\Output\sample.bmp";

        try
        {
            // Verify that the input file exists.
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists.
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the DWF drawing.
            using (Image dwfImage = Image.Load(inputPath))
            {
                // Configure BMP export options.
                BmpOptions bmpOptions = new BmpOptions
                {
                    // Example: set bits per pixel if needed.
                    BitsPerPixel = 24
                };

                // Save the drawing as BMP.
                dwfImage.Save(outputPath, bmpOptions);
                Console.WriteLine($"Export successful: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error during export: {ex.Message}");
        }
    }
}