using Aspose.CAD.FileFormats.Cad;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Validate arguments
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: <input.iges> <output.pdf>");
            return;
        }

        string inputPath = args[0];
        string outputPath = args[1];

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load IGES CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Configure rasterization options
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600f,
                    PageHeight = 1200f
                    // Note: Setting BackgroundColor requires System.Drawing.Color, which is disallowed in this context.
                };

                // Configure PDF export options
                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD image as PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"Successfully converted '{inputPath}' to PDF at '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}