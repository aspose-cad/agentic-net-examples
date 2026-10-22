using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadObjToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define input OBJ file and output PDF file paths
            string inputPath = "input.obj";
            string outputPath = "output.pdf";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: Input file not found at '{inputPath}'.");
                return;
            }

            try
            {
                // Load the OBJ drawing using Aspose.CAD
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Create PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the drawing as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Successfully converted '{inputPath}' to PDF at '{outputPath}'.");
            }
            catch (Exception ex)
            {
                // Print concise error information
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
            }
        }
    }
}