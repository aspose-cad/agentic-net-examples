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
            // Path to the source DWF file
            string sourcePath = "input.dwf";

            // Path where the BMP file will be saved
            string outputPath = "output.bmp";

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

            // Load the DWF drawing using Aspose.CAD.Image
            using (Image dwfImage = Image.Load(sourcePath))
            {
                // Create BMP save options
                BmpOptions bmpOptions = new BmpOptions();

                // Save the loaded drawing directly as a BMP file
                dwfImage.Save(outputPath, bmpOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
    }
}