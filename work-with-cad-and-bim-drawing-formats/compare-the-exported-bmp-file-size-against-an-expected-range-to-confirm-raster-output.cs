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
            string inputPath = "input.dwg";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDir = "output";
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, "output.bmp");

            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options for BMP export
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new[] { "0" } // default layout name
                };

                BmpOptions bmpOptions = new BmpOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, bmpOptions);
            }

            // Verify BMP file size against expected range
            FileInfo bmpInfo = new FileInfo(outputPath);
            long fileSize = bmpInfo.Length;
            long expectedMinSize = 50000;   // 50 KB
            long expectedMaxSize = 5000000; // 5 MB

            if (fileSize >= expectedMinSize && fileSize <= expectedMaxSize)
            {
                Console.WriteLine($"BMP file size {fileSize} bytes is within the expected range.");
            }
            else
            {
                Console.WriteLine($"BMP file size {fileSize} bytes is outside the expected range ({expectedMinSize}-{expectedMaxSize} bytes).");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}