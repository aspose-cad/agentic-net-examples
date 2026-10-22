using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDir = "output";
            Directory.CreateDirectory(outputDir);

            using (Image cadImage = Image.Load(inputPath))
            {
                CadImage cad = (CadImage)cadImage;

                foreach (CadLayout layout in cad.Layouts.Values)
                {
                    // Configure rasterization to render only the current layout
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        Layouts = new string[] { layout.LayoutName },
                        PageWidth = 1600,
                        PageHeight = 1200
                    };

                    // Set PNG export options
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    string outputPath = Path.Combine(
                        outputDir,
                        $"{Path.GetFileNameWithoutExtension(inputPath)}_{layout.LayoutName}.png");

                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"Saved layout '{layout.LayoutName}' to {outputPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}