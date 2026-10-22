using System;
using System.Diagnostics;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadPerformanceDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input CAD file path (replace with actual file location)
                string inputCadPath = "input.dwg";

                // Output image paths
                string defaultOutputPath = "output_default.png";
                string optimizedOutputPath = "output_optimized.png";

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(defaultOutputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the CAD image
                using (Image cadImage = Image.Load(inputCadPath))
                {
                    // -------------------------
                    // Default rendering options
                    // -------------------------
                    var defaultOptions = new PngOptions
                    {
                        VectorRasterizationOptions = new CadRasterizationOptions
                        {
                            PageWidth = 1600,
                            PageHeight = 1200
                            // Default draw type (wireframe) and anti-aliasing are used
                        }
                    };

                    var defaultTimer = Stopwatch.StartNew();
                    cadImage.Save(defaultOutputPath, defaultOptions);
                    defaultTimer.Stop();

                    Console.WriteLine($"Default rendering time: {defaultTimer.ElapsedMilliseconds} ms");

                    // -------------------------
                    // Optimized rendering options
                    // -------------------------
                    var optimizedOptions = new PngOptions
                    {
                        VectorRasterizationOptions = new CadRasterizationOptions
                        {
                            PageWidth = 1600,
                            PageHeight = 1200
                            // No explicit anti-aliasing setting for faster rendering
                        }
                    };

                    var optimizedTimer = Stopwatch.StartNew();
                    cadImage.Save(optimizedOutputPath, optimizedOptions);
                    optimizedTimer.Stop();

                    Console.WriteLine($"Optimized rendering time: {optimizedTimer.ElapsedMilliseconds} ms");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}