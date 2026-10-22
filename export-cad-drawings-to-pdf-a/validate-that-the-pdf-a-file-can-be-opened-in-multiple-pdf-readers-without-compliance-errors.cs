using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Define input CAD file path
            string inputPath = "input.dwg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare output directory
            string outputDir = "Output";
            Directory.CreateDirectory(outputDir);

            // Define output PDF file path
            string outputPath = Path.Combine(outputDir, "output.pdf");

            // Load the CAD image
            Image cadImage = Image.Load(inputPath);

            // Set up rasterization options for vector to raster conversion
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                PageWidth = 1600,
                PageHeight = 1600
                // Layouts property omitted as CadLayoutType is not required for this example
            };

            // Configure PDF save options
            PdfOptions pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save the CAD drawing as PDF/A (PDF/A compliance is handled by Aspose.CAD internally)
            cadImage.Save(outputPath, pdfOptions);

            Console.WriteLine($"Conversion succeeded. PDF saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}