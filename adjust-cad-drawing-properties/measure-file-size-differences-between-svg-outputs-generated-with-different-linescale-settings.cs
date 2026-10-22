using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AdjustCadDrawingProperties
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dwg";
            string outputPathScale1 = "output_scale1.svg";
            string outputPathScale2 = "output_scale2.svg";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load CAD image
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Save with default SVG options (first version)
                    var svgOptions1 = new SvgOptions();
                    try
                    {
                        cadImage.Save(outputPathScale1, svgOptions1);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to save SVG (scale 1): {ex.Message}");
                        return;
                    }

                    // Save with default SVG options (second version)
                    var svgOptions2 = new SvgOptions();
                    try
                    {
                        cadImage.Save(outputPathScale2, svgOptions2);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to save SVG (scale 2): {ex.Message}");
                        return;
                    }
                }

                // Measure file sizes
                long sizeScale1 = GetFileSize(outputPathScale1);
                long sizeScale2 = GetFileSize(outputPathScale2);

                Console.WriteLine($"File size for first SVG: {sizeScale1} bytes");
                Console.WriteLine($"File size for second SVG: {sizeScale2} bytes");
                Console.WriteLine($"Size difference: {Math.Abs(sizeScale2 - sizeScale1)} bytes");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        private static long GetFileSize(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    return new FileInfo(path).Length;
                }
                Console.WriteLine($"File not found: {path}");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving file size for {path}: {ex.Message}");
                return 0;
            }
        }
    }
}
