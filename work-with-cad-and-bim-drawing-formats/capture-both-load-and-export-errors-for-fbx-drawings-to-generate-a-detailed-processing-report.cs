using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define input and output paths
            string inputFilePath = "input.dwg";
            string outputDirectory = "output";
            string outputFilePath = Path.Combine(outputDirectory, "output.pdf");

            // Verify that the input file exists
            if (!File.Exists(inputFilePath))
            {
                Console.Error.WriteLine($"Input file not found: {inputFilePath}");
                return;
            }

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            try
            {
                // Load the CAD drawing
                using (Image cadImage = Image.Load(inputFilePath))
                {
                    // Set PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Optional: configure rasterization options if needed
                    // pdfOptions.VectorRasterizationOptions = new CadRasterizationOptions
                    // {
                    //     PageWidth = 1600,
                    //     PageHeight = 1600,
                    //     Layouts = new string[] { "Model" } // Example layout name
                    // };

                    // Save the drawing as PDF
                    cadImage.Save(outputFilePath, pdfOptions);
                }

                Console.WriteLine($"CAD file successfully converted to PDF: {outputFilePath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred during processing: {ex.Message}");
            }
        }
    }
}