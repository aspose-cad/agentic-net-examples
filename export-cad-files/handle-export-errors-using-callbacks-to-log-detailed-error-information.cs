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
                // Define input CAD file path (placeholder)
                string inputFilePath = "sample.dwg";

                // Verify that the input file exists
                if (!File.Exists(inputFilePath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputFilePath}");
                    return;
                }

                // Create output directory
                string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the CAD image using Aspose.CAD.Image.Load
                using (Image cadImage = Image.Load(inputFilePath))
                {
                    // Export to PDF
                    string pdfPath = Path.Combine(outputDir, "output.pdf");
                    PdfOptions pdfOptions = new PdfOptions();
                    cadImage.Save(pdfPath, pdfOptions);
                    Console.WriteLine($"Saved PDF to: {pdfPath}");

                    // Export to SVG
                    string svgPath = Path.Combine(outputDir, "output.svg");
                    SvgOptions svgOptions = new SvgOptions();
                    cadImage.Save(svgPath, svgOptions);
                    Console.WriteLine($"Saved SVG to: {svgPath}");

                    // Export to PNG
                    string pngPath = Path.Combine(outputDir, "output.png");
                    PngOptions pngOptions = new PngOptions();
                    cadImage.Save(pngPath, pngOptions);
                    Console.WriteLine($"Saved PNG to: {pngPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}