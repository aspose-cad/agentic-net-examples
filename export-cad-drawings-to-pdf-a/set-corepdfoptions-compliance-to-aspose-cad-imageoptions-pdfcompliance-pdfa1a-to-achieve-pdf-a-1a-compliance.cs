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
                // Define input and output paths
                string inputFile = "input.dwg";
                string outputDirectory = "Output";
                string outputFile = Path.Combine(outputDirectory, "output.pdf");

                // Verify input file exists
                if (!File.Exists(inputFile))
                {
                    Console.Error.WriteLine($"Input file not found: {inputFile}");
                    return;
                }

                // Ensure output directory exists
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Load the CAD drawing
                using (Image cadImage = Image.Load(inputFile))
                {
                    // Set up PDF save options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Optional: control rasterization of vector data
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1600,
                        // Render only the Model layout; adjust as needed
                        Layouts = new[] { "Model" }
                    };

                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the drawing as PDF
                    cadImage.Save(outputFile, pdfOptions);
                }

                Console.WriteLine($"Conversion completed successfully. PDF saved to: {outputFile}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}