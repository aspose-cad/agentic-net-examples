using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadRenderingAdjustment
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.cf2";
            string outputPath = "output.png";

            try
            {
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                using (Image loadedImage = Image.Load(inputPath))
                {
                    // Cast to CadImage for CAD-specific properties
                    CadImage cadImage = (CadImage)loadedImage;

                    // Configure rasterization options to improve rendering of complex models
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 3000f,
                        PageHeight = 3000f,
                        UnitType = UnitType.Centimenter,
                        LineScale = 1.0f,
                        AutomaticLayoutsScaling = false,
                        Layouts = null,
                        Margins = new Margins { Left = 0, Right = 0, Top = 0, Bottom = 0 },
                        Quality = new RasterizationQuality
                        {
                            Text = RasterizationQualityValue.High,
                            Hatch = RasterizationQualityValue.High,
                            Arc = RasterizationQualityValue.High
                        }
                    };

                    // Set up PNG export options with the rasterization settings
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the rendered image
                    cadImage.Save(outputPath, pngOptions);
                }

                Console.WriteLine($"Rendering completed. Output saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}