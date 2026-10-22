using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace AdjustCadDrawingProperties
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input CAD file path
            string inputPath = "sample.dwg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Output SVG file paths
            string defaultSvgPath = "output_default.svg";
            string increasedSvgPath = "output_increased.svg";

            try
            {
                // Load the CAD drawing
                using (Image cadImage = Image.Load(inputPath))
                {
                    // ---------- Save with default LineScale ----------
                    var defaultRasterOptions = new CadRasterizationOptions
                    {
                        LineScale = 1.0f
                    };
                    var defaultSvgOptions = new SvgOptions
                    {
                        VectorRasterizationOptions = defaultRasterOptions
                    };

                    try
                    {
                        cadImage.Save(defaultSvgPath, defaultSvgOptions);
                        Console.WriteLine($"Saved default SVG to: {defaultSvgPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error saving default SVG: {ex.Message}");
                    }

                    // ---------- Save with increased LineScale ----------
                    var increasedRasterOptions = new CadRasterizationOptions
                    {
                        LineScale = 5.0f
                    };
                    var increasedSvgOptions = new SvgOptions
                    {
                        VectorRasterizationOptions = increasedRasterOptions
                    };

                    try
                    {
                        cadImage.Save(increasedSvgPath, increasedSvgOptions);
                        Console.WriteLine($"Saved increased LineScale SVG to: {increasedSvgPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error saving increased SVG: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading CAD file: {ex.Message}");
            }
        }
    }
}
