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
        string inputPath = @"C:\CAD\sample.dwg";
        string outputPath = @"C:\CAD\Export\sample.pdf";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Log external file references (e.g., raster image definitions)
                Console.WriteLine("External file references found:");
                bool anyExternal = false;
                foreach (var obj in cadImage.Objects)
                {
                    if (obj is CadRasterImageDef rasterDef)
                    {
                        anyExternal = true;
                        Console.WriteLine($"- {rasterDef.FileName}");
                    }
                }
                if (!anyExternal)
                {
                    Console.WriteLine("None");
                }

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                var rasterizationOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200
                    // BackgroundColor omitted to avoid System.Drawing usage
                };

                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterizationOptions
                };

                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"CAD file successfully exported to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during processing: {ex.Message}");
        }
    }
}