using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main(string[] args)
    {
        // Input IGES file path (first argument) and output directory (second argument)
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: Program <input-iges-file> <output-directory>");
            return;
        }

        string inputPath = args[0];
        string outputDir = args[1];

        // Validate input file
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Error: Input file '{inputPath}' does not exist.");
            return;
        }

        // Validate IGES extension
        string ext = Path.GetExtension(inputPath).ToLowerInvariant();
        if (ext != ".iges" && ext != ".igs")
        {
            Console.WriteLine("Error: Input file must be an IGES file with .iges or .igs extension.");
            return;
        }

        // Ensure output directory exists
        if (!Directory.Exists(outputDir))
        {
            try
            {
                Directory.CreateDirectory(outputDir);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Unable to create output directory. {ex.Message}");
                return;
            }
        }

        // Prepare output PDF path
        string outputFileName = Path.GetFileNameWithoutExtension(inputPath) + ".pdf";
        string outputPath = Path.Combine(outputDir, outputFileName);

        try
        {
            // Load the IGES file
            Image cadImage = Image.Load(inputPath);

            // Configure rasterization options
            var rasterOptions = new CadRasterizationOptions
            {
                // Set background color to black using Aspose.CAD's Color
                BackgroundColor = Color.Black,

                // Use the actual image dimensions to avoid zero sizes
                PageWidth = cadImage.Width,
                PageHeight = cadImage.Height,

                // Optional: set unit type and line scale if needed
                UnitType = UnitType.Centimenter,
                LineScale = 1.0f
            };

            // Configure PDF export options
            var pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save as PDF
            cadImage.Save(outputPath, pdfOptions);

            Console.WriteLine($"Successfully converted IGES to PDF: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Conversion failed: {ex.Message}");
        }
    }
}