using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Determine input file path (first argument or default name)
        string inputPath = args.Length > 0 ? args[0] : "sample.dxb";

        // Verify that the input file exists
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine("Input file not found: " + inputPath);
            return;
        }

        // Prepare output directory
        string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "output");
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        // Build output file path with .pdf extension
        string outputPath = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(inputPath) + ".pdf");

        try
        {
            // Load the DXB file using Aspose.CAD
            using (Image cadImage = Image.Load(inputPath))
            {
                // Create PDF save options (default settings)
                PdfOptions pdfOptions = new PdfOptions();

                // Save the image as PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine("Conversion successful. PDF saved to: " + outputPath);
        }
        catch (Exception ex)
        {
            // Write any errors to the error stream
            Console.Error.WriteLine("Error during conversion: " + ex.Message);
        }
    }
}