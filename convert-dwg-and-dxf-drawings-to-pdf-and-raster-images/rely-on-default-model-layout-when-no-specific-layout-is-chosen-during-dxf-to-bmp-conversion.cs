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
            try
            {
                // Define input CAD file path
                string inputFilePath = "input.dwg";

                // Verify that the input file exists
                if (!File.Exists(inputFilePath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputFilePath}");
                    return;
                }

                // Define output directory and file path
                string outputDirectory = "output";
                Directory.CreateDirectory(outputDirectory);
                string outputFilePath = Path.Combine(outputDirectory, "output.pdf");

                // Load the CAD image
                using (Image cadImage = Image.Load(inputFilePath))
                {
                    // Set up PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Optional: configure rasterization options if needed
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    // Use default layout; no specific layouts or layers are set
                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the CAD image as PDF
                    cadImage.Save(outputFilePath, pdfOptions);
                }

                Console.WriteLine($"Conversion completed successfully. Output saved to: {outputFilePath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}