using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadMarginExample
{
    class Program
    {
        static void Main()
        {
            // Path to the source CAD file (DWG, DXF, etc.)
            string inputPath = Path.Combine(Directory.GetCurrentDirectory(), "input.dwg");

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input CAD file not found at: {inputPath}");
                return;
            }

            // Prepare output directory and file path
            string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "output");
            Directory.CreateDirectory(outputDirectory);
            string outputPath = Path.Combine(outputDirectory, "result.pdf");

            try
            {
                // Load the CAD drawing
                using (var cadImage = (Image)Image.Load(inputPath))
                {
                    // Configure rasterization options (larger page size simulates margins)
                    var rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1300,   // Width in pixels (including desired margins)
                        PageHeight = 900    // Height in pixels (including desired margins)
                    };

                    // Set up PDF save options and attach rasterization settings
                    var pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the drawing as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"PDF successfully created at: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
