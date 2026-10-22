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
            // Define input IGES file and output PDF file paths
            string inputPath = "sample.iges";
            string outputPath = "output.pdf";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the IGES file using Aspose.CAD
            using (Image cadImage = Image.Load(inputPath))
            {
                // Create PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Optional: configure rasterization options if needed
                // pdfOptions.VectorRasterizationOptions = new CadRasterizationOptions
                // {
                //     PageWidth = 800,
                //     PageHeight = 600
                // };

                // Save the loaded CAD image as PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"PDF file saved successfully to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}