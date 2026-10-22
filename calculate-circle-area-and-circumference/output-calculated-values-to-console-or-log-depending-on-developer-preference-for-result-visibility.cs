using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.dwg";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDir = "Output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (Image cadImage = Image.Load(inputPath))
            {
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.Layouts = new string[] { "Model" };
                rasterOptions.PageWidth = 1600;
                rasterOptions.PageHeight = 1200;
                rasterOptions.DrawType = CadDrawTypeMode.UseObjectColor;

                PngOptions pngOptions = new PngOptions();
                pngOptions.VectorRasterizationOptions = rasterOptions;

                string outputPath = Path.Combine(outputDir, "sample.png");
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Image saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
