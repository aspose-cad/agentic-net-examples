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
            // Define input and output file paths
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Set up PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Optional: configure rasterization options for vector-to-raster conversion
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.PageWidth = 1600;   // Desired page width in pixels
                rasterOptions.PageHeight = 1200;  // Desired page height in pixels
                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save the CAD drawing as PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"File successfully saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            // Print concise error information
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}