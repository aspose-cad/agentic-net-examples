using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath1 = "output1.png";
        string outputPath2 = "output2.png";
        string customFontFolder = @"C:\CustomFonts";

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directories exist
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath1) ?? ".");
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath2) ?? ".");

            // First rendering session with custom font folder
            using (CadImage cadImage1 = (CadImage)Image.Load(inputPath))
            {
                cadImage1.DefaultFont = customFontFolder;

                var pngOptions1 = new PngOptions();
                var rasterOptions1 = new CadRasterizationOptions
                {
                    PageWidth = 1600f,
                    PageHeight = 1200f,
                    Layouts = cadImage1.Layouts.Keys.ToArray()
                };
                pngOptions1.VectorRasterizationOptions = rasterOptions1;

                cadImage1.Save(outputPath1, pngOptions1);
            }

            // Second rendering session using the same custom font folder
            using (CadImage cadImage2 = (CadImage)Image.Load(inputPath))
            {
                cadImage2.DefaultFont = customFontFolder;

                var pngOptions2 = new PngOptions();
                var rasterOptions2 = new CadRasterizationOptions
                {
                    PageWidth = 1600f,
                    PageHeight = 1200f,
                    Layouts = cadImage2.Layouts.Keys.ToArray()
                };
                pngOptions2.VectorRasterizationOptions = rasterOptions2;

                cadImage2.Save(outputPath2, pngOptions2);
            }

            Console.WriteLine("Rendering completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}