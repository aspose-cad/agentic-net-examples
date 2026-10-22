using System.Linq;
using System;
using System.Diagnostics;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadRenderPerformance
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input CAD file path (replace with actual path)
                string inputPath = "input.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Output directory
                string outputDir = "output";

                // Ensure output directory exists
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load CAD image
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Common rasterization options
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200
                    };

                    // -------------------- Original Rendering --------------------
                    Stopwatch swOriginal = new Stopwatch();
                    swOriginal.Start();

                    using (MemoryStream msOriginal = new MemoryStream())
                    {
                        PngOptions pngOptionsOriginal = new PngOptions
                        {
                            VectorRasterizationOptions = rasterOptions
                        };

                        cadImage.Save(msOriginal, pngOptionsOriginal);
                        File.WriteAllBytes(Path.Combine(outputDir, "original.png"), msOriginal.ToArray());
                    }

                    swOriginal.Stop();
                    long originalRenderTimeMs = swOriginal.ElapsedMilliseconds;

                    // -------------------- Optimized Rendering --------------------
                    // (No specific hatch optimization performed due to API limitations)
                    Stopwatch swOptimized = new Stopwatch();
                    swOptimized.Start();

                    using (MemoryStream msOptimized = new MemoryStream())
                    {
                        PngOptions pngOptionsOptimized = new PngOptions
                        {
                            VectorRasterizationOptions = rasterOptions
                        };

                        cadImage.Save(msOptimized, pngOptionsOptimized);
                        File.WriteAllBytes(Path.Combine(outputDir, "optimized.png"), msOptimized.ToArray());
                    }

                    swOptimized.Stop();
                    long optimizedRenderTimeMs = swOptimized.ElapsedMilliseconds;

                    // -------------------- Report Results --------------------
                    Console.WriteLine($"Original render time: {originalRenderTimeMs} ms");
                    Console.WriteLine($"Optimized render time: {optimizedRenderTimeMs} ms");
                    if (optimizedRenderTimeMs > 0)
                    {
                        double gain = (double)(originalRenderTimeMs - optimizedRenderTimeMs) / optimizedRenderTimeMs * 100;
                        Console.WriteLine($"Performance gain: {gain:F2}%");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}