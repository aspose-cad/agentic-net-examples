using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadStlToPng
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputPath = "input.stl";
                string outputPath = "output.png";

                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                using (Image cadImage = Image.Load(inputPath))
                {
                    // Configure rasterization options (optional)
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1000,
                        PageHeight = 1000,
                        // You can set additional options such as background color, layouts, etc., if needed
                    };

                    // Configure PNG export options
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the image as PNG
                    cadImage.Save(outputPath, pngOptions);
                }

                Console.WriteLine($"Successfully converted '{inputPath}' to '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}