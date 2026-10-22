using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main(string[] args)
    {
        string inputPath = args.Length > 0 ? args[0] : "sample.dwg";
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
        try
        {
            Directory.CreateDirectory(outputDir);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
            return;
        }

        string outputPath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(inputPath) + ".pdf");

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Loaded file is not a CAD image.");
                    return;
                }

                // Optional: iterate filtered entities (lines and texts) for any custom processing
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadLine line)
                    {
                        // Access line properties if needed, e.g., line.FirstPoint, line.SecondPoint, line.ColorId
                    }
                    else if (entity is CadText text)
                    {
                        // Access text properties if needed, e.g., text.TextString, text.InsertionPoint, text.ColorId
                    }
                }

                var pdfOptions = new PdfOptions();
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200
                    // No EntityTypes property; all entities will be rendered preserving original colors
                };
                pdfOptions.VectorRasterizationOptions = rasterOptions;

                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"Export completed: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}