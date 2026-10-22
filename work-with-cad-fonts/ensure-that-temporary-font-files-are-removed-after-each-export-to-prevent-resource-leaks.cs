using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";
            string outputPath = "output.png";
            string tempFontPath = "tempFont.ttf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            if (!File.Exists(tempFontPath))
            {
                Console.Error.WriteLine($"Temporary font file not found: {tempFontPath}");
                return;
            }

            using (Image cadImage = Image.Load(inputPath))
            {
                CadImage cad = cadImage as CadImage;
                if (cad != null)
                {
                    cad.DefaultFont = tempFontPath;
                }

                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new[] { "Model" }
                };

                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, pngOptions);
            }

            try
            {
                File.Delete(tempFontPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to delete temporary font file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}