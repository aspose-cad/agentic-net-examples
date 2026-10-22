using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string sourcePath = "input.dwg";
        string outputPath = "output.png";

        if (!File.Exists(sourcePath))
        {
            Console.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        try
        {
            using (Image cadImage = Image.Load(sourcePath))
            {
                CadImage cad = (CadImage)cadImage;

                PngOptions pngOptions = new PngOptions();

                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layouts = new string[] { "Model" },
                    PageWidth = 1600,
                    PageHeight = 1200
                    // BackgroundColor omitted to avoid System.Drawing usage
                };

                pngOptions.VectorRasterizationOptions = rasterOptions;

                try
                {
                    string outputDir = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"Image saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving image: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading CAD file: {ex.Message}");
        }
    }
}
