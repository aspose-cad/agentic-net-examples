using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadToPdfExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define input and output file paths
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

            // Verify that the input DWG file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the DWG file using Aspose.CAD
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Configure rasterization options (page size can be adjusted to create margins)
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1200,   // Adjust as needed
                        PageHeight = 800    // Adjust as needed
                    };

                    // Set up PDF export options and attach rasterization settings
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the rasterized result to PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"DWG file successfully converted to PDF: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during conversion: {ex.Message}");
            }
        }
    }
}
