using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input CAD file path (replace with actual path)
            string inputPath = "input.dwg";

            // Output file paths
            string svgPath = "output.svg";
            string pngPath = "output.png";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Export to SVG (LineScale not available in current SDK version)
                    try
                    {
                        SvgOptions svgOptions = new SvgOptions();
                        cadImage.Save(svgPath, svgOptions);
                        Console.WriteLine($"SVG saved to {svgPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error saving SVG: {ex.Message}");
                    }

                    // Export to PNG without any SVG-specific settings
                    try
                    {
                        PngOptions pngOptions = new PngOptions();
                        cadImage.Save(pngPath, pngOptions);
                        Console.WriteLine($"PNG saved to {pngPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error saving PNG: {ex.Message}");
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
