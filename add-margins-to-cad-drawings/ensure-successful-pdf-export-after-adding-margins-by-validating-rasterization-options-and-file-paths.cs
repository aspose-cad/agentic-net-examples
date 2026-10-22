using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Path to the source CAD file
        string inputFilePath = "input.dwg";

        // Path for the resulting PDF file
        string outputFilePath = "output.pdf";

        // Verify input file exists
        if (!File.Exists(inputFilePath))
        {
            Console.WriteLine($"Input file not found: {inputFilePath}");
            return;
        }

        // Ensure the output directory exists
        try
        {
            string outputDirectory = Path.GetDirectoryName(outputFilePath);
            if (!string.IsNullOrEmpty(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to create output directory: {ex.Message}");
            return;
        }

        // Load the CAD drawing and export to PDF with margins
        try
        {
            using (Image cadImage = Image.Load(inputFilePath))
            {
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1200,
                    PageHeight = 800
                };

                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputFilePath, pdfOptions);
                Console.WriteLine($"PDF successfully saved to: {outputFilePath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}
