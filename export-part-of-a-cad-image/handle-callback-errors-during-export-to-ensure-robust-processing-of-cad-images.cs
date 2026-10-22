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
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Define output PDF file path
            string outputPath = Path.Combine(outputDir, "output.pdf");

            // Load the CAD image
            using (Image cadImage = Image.Load(inputPath))
            {
                // Set PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Attempt to save the CAD image as PDF and handle any export errors
                try
                {
                    cadImage.Save(outputPath, pdfOptions);
                    Console.WriteLine($"CAD file exported successfully to: {outputPath}");
                }
                catch (Exception exportEx)
                {
                    Console.Error.WriteLine($"Error during export: {exportEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            // Catch any unexpected errors in the overall process
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}