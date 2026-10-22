using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadCf2ToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input and output paths
                string inputPath = "input.cf2";
                string outputDirectory = "Output";
                string outputPath = Path.Combine(outputDirectory, "output.pdf");

                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Ensure the output directory exists
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Load the CF2 file
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Set PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the CAD image as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Conversion completed successfully. PDF saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during conversion: {ex.Message}");
            }
        }
    }
}