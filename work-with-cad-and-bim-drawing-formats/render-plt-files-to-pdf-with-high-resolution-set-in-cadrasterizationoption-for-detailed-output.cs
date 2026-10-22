using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats;

namespace AsposeCadPltToPdf
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define input and output paths
                string inputPath = "input.plt";
                string outputDirectory = "output";
                string outputPath = Path.Combine(outputDirectory, "rendered.pdf");

                // Verify that the input PLT file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Ensure the output directory exists
                Directory.CreateDirectory(outputDirectory);

                // Load the PLT file
                Image cadImage = Image.Load(inputPath);

                // Configure rasterization options for high‑resolution rendering
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Define page size (in points; 1 point = 1/72 inch)
                    // For A4 at 300 DPI: 8.27in * 300 = 2481 pixels, 8.27in * 72 = 595 points width
                    // Height: 11.69in * 300 = 3507 pixels, 11.69in * 72 = 842 points height
                    PageWidth = 595f,   // Approx. A4 width in points
                    PageHeight = 842f,  // Approx. A4 height in points

                    // Observer point defines rotation angles (X, Y, Z) in degrees
                    ObserverPoint = new ObserverPoint(0f, 0f, 0f)
                };

                // Set PDF export options and attach rasterization settings
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the rendered PDF
                cadImage.Save(outputPath, pdfOptions);

                Console.WriteLine($"PDF successfully created at: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}