using System;
using System.IO;
using System.Diagnostics;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image cadImage = Image.Load(inputPath))
            {
                // Simulated quality levels (1 = Low, 2 = Medium, 3 = High)
                int[] qualityLevels = new int[] { 1, 2, 3 };

                foreach (int q in qualityLevels)
                {
                    var rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 800,
                        PageHeight = 600,
                        // Use LineScale as a simple proxy for quality (lower value = lower quality)
                        LineScale = q
                    };

                    var pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    string outputPath = $"output_hatch_{q}.png";

                    // Ensure the output directory exists
                    string outputDir = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    Stopwatch sw = Stopwatch.StartNew();
                    cadImage.Save(outputPath, pngOptions);
                    sw.Stop();

                    Console.WriteLine($"Saved {outputPath} with proxy quality={q} in {sw.ElapsedMilliseconds} ms");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}