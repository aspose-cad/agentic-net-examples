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
        try
        {
            string inputPath = "input.dwg";
            string outputWithFont = "output_with_font.png";
            string outputWithoutFont = "output_without_font.png";
            string customFontFolder = @"C:\CustomFonts";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Export without custom font folder
                ExportToPng(cadImage, outputWithoutFont);

                // Set custom font folder path
                cadImage.DefaultFont = customFontFolder;

                // Export with custom font folder
                ExportToPng(cadImage, outputWithFont);
            }

            Console.WriteLine("Export completed.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    static void ExportToPng(CadImage cadImage, string outputPath)
    {
        // Ensure output directory exists
        string? outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Configure rasterization options
        CadRasterizationOptions rasterOptions = new CadRasterizationOptions
        {
            PageWidth = 1600f,
            PageHeight = 1200f,
            Layouts = cadImage.Layouts.Keys.ToArray()
        };

        // Configure PNG options
        PngOptions pngOptions = new PngOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        // Save the image
        cadImage.Save(outputPath, pngOptions);
    }
}