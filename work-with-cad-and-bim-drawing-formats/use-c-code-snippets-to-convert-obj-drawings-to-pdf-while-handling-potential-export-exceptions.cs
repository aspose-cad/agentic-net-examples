using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Input OBJ file and desired PDF output paths
        string inputFilePath = "input.obj";
        string outputFilePath = "output.pdf";

        // Verify input file exists
        if (!File.Exists(inputFilePath))
        {
            Console.WriteLine($"Input file not found: {inputFilePath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputFilePath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the OBJ drawing
            using (Image cadImage = Image.Load(inputFilePath))
            {
                // Configure PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Export the drawing to PDF
                cadImage.Save(outputFilePath, pdfOptions);
                Console.WriteLine("Conversion succeeded: " + outputFilePath);
            }
        }
        catch (Exception ex)
        {
            // Handle any errors that occur during export
            Console.WriteLine("Error during conversion: " + ex.Message);
        }
    }
}