using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadStlToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input STL file and output PDF file paths
                string inputPath = "input.stl";
                string outputPath = "output.pdf";

                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the STL file using Aspose.CAD
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Configure PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the loaded CAD image as a PDF file
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Successfully exported '{inputPath}' to '{outputPath}'.");
            }
            catch (Exception ex)
            {
                // Print concise error information
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}