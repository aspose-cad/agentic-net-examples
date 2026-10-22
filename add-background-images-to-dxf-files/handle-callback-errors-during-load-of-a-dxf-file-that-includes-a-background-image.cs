using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadBackgroundImageExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input DXF file that may contain background images
            string inputFile = "input.dxf";
            // Desired output PDF file
            string outputFile = "output.pdf";

            // Verify that the input file exists
            if (!File.Exists(inputFile))
            {
                Console.WriteLine($"Input file not found: {inputFile}");
                return;
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputFile);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                // Load the DXF file (any load errors will be caught by the outer try/catch)
                using (Image cadImage = Image.Load(inputFile))
                {
                    // Set PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the loaded CAD image as PDF
                    cadImage.Save(outputFile, pdfOptions);
                }

                Console.WriteLine("DXF file loaded and saved to PDF successfully.");
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors during load or save
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
