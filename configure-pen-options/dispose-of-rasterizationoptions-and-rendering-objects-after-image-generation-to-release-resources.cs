using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Expect input CAD file path and output PDF file path as arguments
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: Program <input-cad-file> <output-pdf-file>");
            return;
        }

        string inputPath = args[0];
        string outputPath = args[1];

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file does not exist: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outputDirectory = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        try
        {
            // Load CAD image
            using (Image cadImage = Image.Load(inputPath))
            {
                // Prepare PDF export options
                var pdfOptions = new PdfOptions();

                // Configure vector rasterization options (concrete implementation)
                var vectorOptions = new CadRasterizationOptions
                {
                    PageWidth = 595,   // A4 width in points (8.27 in * 72)
                    PageHeight = 842,  // A4 height in points (11.69 in * 72)
                    BackgroundColor = Aspose.CAD.Color.White
                };

                pdfOptions.VectorRasterizationOptions = vectorOptions;

                // Save CAD as PDF
                cadImage.Save(outputPath, pdfOptions);

                // Release rasterization resources
                vectorOptions = null;
            }

            Console.WriteLine($"Conversion successful: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}