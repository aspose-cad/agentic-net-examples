using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace AsposeCadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dxf";
            string outputPath = "output.png";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
                return;
            }

            try
            {
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Rasterization options using default scaling (ShrinkToFit)
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        UnitType = UnitType.Centimenter // SDK spelling
                        // No explicit PageWidth/PageHeight set to rely on default scaling behavior
                    };

                    // PNG export options
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"Image saved to {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}