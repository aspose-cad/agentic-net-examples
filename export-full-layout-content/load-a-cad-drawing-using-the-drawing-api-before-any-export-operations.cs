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
            // Path to the source CAD file
            string sourcePath = "input.dwg";

            // Verify that the source file exists
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            // Load the CAD drawing using Aspose.CAD
            using (Image cadImage = Image.Load(sourcePath))
            {
                // Example operation: display basic image dimensions
                int width = cadImage.Width;
                int height = cadImage.Height;
                Console.WriteLine($"CAD drawing loaded. Width: {width}, Height: {height}");

                // Optional export to PNG (demonstrates export after loading)
                string outputPath = "output.png";

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                PngOptions pngOptions = new PngOptions();
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Exported PNG to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}