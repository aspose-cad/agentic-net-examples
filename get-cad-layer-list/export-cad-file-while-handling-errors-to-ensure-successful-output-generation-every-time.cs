using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define input CAD file path (replace with actual file path)
            string inputFilePath = "input.dwg";

            // Define output directory and file name
            string outputDirectory = "Output";
            string outputFileName = "output.pdf";
            string outputFilePath = Path.Combine(outputDirectory, outputFileName);

            try
            {
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

                // Load the CAD image
                using (Image cadImage = Image.Load(inputFilePath))
                {
                    // Set up PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Optional: configure rasterization options if needed
                    // CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    // rasterOptions.PageWidth = 800;
                    // rasterOptions.PageHeight = 600;
                    // pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the CAD drawing as PDF
                    cadImage.Save(outputFilePath, pdfOptions);
                }

                Console.WriteLine($"CAD file successfully exported to: {outputFilePath}");
            }
            catch (Exception ex)
            {
                // Print concise error information without crashing
                Console.Error.WriteLine($"An error occurred during export: {ex.Message}");
            }
        }
    }
}