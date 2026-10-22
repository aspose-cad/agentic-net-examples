using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the source CAD file
            using (Image loadedImage = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD‑specific members
                CadImage cadImage = (CadImage)loadedImage;

                // Substitute missing fonts with the specified default font
                cadImage.DefaultFont = "Arial";

                // Define PNG export options
                PngOptions pngOptions = new PngOptions();

                // Export the CAD drawing to PNG using the substituted font
                cadImage.Save(outputPath, pngOptions);
            }

            // Validate that the exported image file was created and has content
            FileInfo exportedInfo = new FileInfo(outputPath);
            if (exportedInfo.Exists && exportedInfo.Length > 0)
            {
                Console.WriteLine("Export succeeded: the image was generated and contains data.");
                // Additional visual inspection can be performed to confirm correct font rendering.
            }
            else
            {
                Console.WriteLine("Export failed: the image file was not created correctly.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}