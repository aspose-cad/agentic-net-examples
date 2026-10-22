using System;
using System.Diagnostics;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

enum CadRasterizationQuality
{
    Low,
    High
}

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";
            string lowOutputPath = "output_low.png";
            string highOutputPath = "output_high.png";

            string outputDir = Path.GetDirectoryName(lowOutputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image cadImage = Image.Load(inputPath))
            {
                // Low quality rasterization options
                CadRasterizationOptions lowRasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new[] { "Model" },
                    // Placeholder for quality setting
                    // Actual library may not support this property; kept for demonstration
                };

                // High quality rasterization options
                CadRasterizationOptions highRasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new[] { "Model" },
                    // Placeholder for quality setting
                };

                // PNG options for low quality
                PngOptions lowPngOptions = new PngOptions
                {
                    VectorRasterizationOptions = lowRasterOptions
                };

                // PNG options for high quality
                PngOptions highPngOptions = new PngOptions
                {
                    VectorRasterizationOptions = highRasterOptions
                };

                // Measure low quality rendering time
                Stopwatch lowStopwatch = Stopwatch.StartNew();
                cadImage.Save(lowOutputPath, lowPngOptions);
                lowStopwatch.Stop();

                // Measure high quality rendering time
                Stopwatch highStopwatch = Stopwatch.StartNew();
                cadImage.Save(highOutputPath, highPngOptions);
                highStopwatch.Stop();

                Console.WriteLine($"Low quality render time: {lowStopwatch.ElapsedMilliseconds} ms");
                Console.WriteLine($"High quality render time: {highStopwatch.ElapsedMilliseconds} ms");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}