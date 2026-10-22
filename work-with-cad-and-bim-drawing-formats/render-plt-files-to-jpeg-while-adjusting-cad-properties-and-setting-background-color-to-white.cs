using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "input.plt";
        string outputPath = "output.jpg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the PLT file. For PLT, Image.Load returns a generic Image, not a CadImage.
            using (Image image = Image.Load(inputPath))
            {
                // CAD-specific properties such as DefaultFont are unavailable for non‑CadImage formats.
                // Setting a background color requires System.Drawing.Color, which is prohibited in this sample.
                // Therefore, we proceed with default rendering options.

                var jpegOptions = new JpegOptions();
                // No background color can be set without using System.Drawing.

                image.Save(outputPath, jpegOptions);
                Console.WriteLine($"Rendered JPEG saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}