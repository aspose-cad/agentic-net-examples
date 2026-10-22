using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadRenderWithCustomFont
{
    class Program
    {
        static void Main()
        {
            string inputPath = "input.dwg";
            string outputPath = "output.png";
            string fontPath = @"C:\CustomFonts\MyFont.ttf";

            try
            {
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                if (!File.Exists(fontPath))
                {
                    Console.Error.WriteLine($"Font file not found: {fontPath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                using (Image cadImage = Image.Load(inputPath))
                {
                    // Cast to CadImage to set the default font for missing fonts
                    CadImage cad = cadImage as CadImage;
                    if (cad != null)
                    {
                        cad.DefaultFont = fontPath;

                        // Configure rasterization options
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                        {
                            PageWidth = 1600f,
                            PageHeight = 1200f,
                            // Use all available layout names in the CAD file
                            Layouts = cad.Layouts.Keys.ToArray()
                        };

                        // Set PNG output options
                        PngOptions pngOptions = new PngOptions
                        {
                            VectorRasterizationOptions = rasterOptions
                        };

                        // Save the rendered image
                        cadImage.Save(outputPath, pngOptions);
                    }
                    else
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    }
                }

                Console.WriteLine($"Rendered image saved to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}