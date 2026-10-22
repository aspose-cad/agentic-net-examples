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
            // Placeholder input and output file paths
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: Input file \"{inputPath}\" does not exist.");
                return;
            }

            try
            {
                // Load the CAD file using Aspose.CAD.Image.Load
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Set up PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the CAD image as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Successfully converted \"{inputPath}\" to \"{outputPath}\".");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}