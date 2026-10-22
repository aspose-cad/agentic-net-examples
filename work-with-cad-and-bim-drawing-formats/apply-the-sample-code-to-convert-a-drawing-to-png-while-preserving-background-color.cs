using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class ConvertCadToPng
{
    static void Main()
    {
        // Path to the source CAD drawing
        string sourcePath = @"input.dwg";

        // Path for the resulting PNG image
        string outputPath = @"output.png";

        // Verify source file exists
        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD drawing
            using (Image cadImage = Image.Load(sourcePath))
            {
                // Prepare PNG export options
                var pngOptions = new PngOptions();
                // Note: Aspose.CAD.PngOptions does not expose a BackgroundColor property.
                // Background color preservation is handled by the default rendering settings.

                // Save the drawing as PNG using the specified options
                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}