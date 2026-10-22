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
            // Define input and output file paths
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the CAD image
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Configure rasterization options for vector output
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Optional: set page size or other rasterization settings
                        PageWidth = 1600,
                        PageHeight = 1200,
                        // You can also set Layouts, Layers, etc., if needed
                    };

                    // Set up PDF export options
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the CAD drawing as a PDF file
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"CAD file successfully converted to PDF: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}