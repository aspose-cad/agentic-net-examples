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
            string inputPath = "sample.dwf";
            string outputPath = "output.bmp";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD image
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options for BMP output
                var rasterOptions = new CadRasterizationOptions
                {
                    // UnitType.Centimenter is the correct enum spelling in the SDK
                    UnitType = UnitType.Centimenter,
                    PageWidth = 800,
                    PageHeight = 600,
                    LineScale = 1.0f
                };

                var bmpOptions = new BmpOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save as BMP
                cadImage.Save(outputPath, bmpOptions);
            }

            // Verify that the BMP file was created and has non‑zero size
            if (!File.Exists(outputPath))
            {
                Console.Error.WriteLine($"Output BMP not created: {outputPath}");
                return;
            }

            FileInfo info = new FileInfo(outputPath);
            if (info.Length > 0)
            {
                Console.WriteLine($"BMP conversion successful. File size: {info.Length} bytes");
            }
            else
            {
                Console.Error.WriteLine("BMP conversion resulted in an empty file.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}