using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define input and output file paths
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

            try
            {
                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD file
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD‑specific properties
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // Set a default font to be used when required fonts are missing
                    cadImage.DefaultFont = "Arial";

                    // Prepare PDF save options (or any other format that requires fonts)
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the CAD drawing to PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Export completed successfully. Output saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                // Print concise error information
                Console.Error.WriteLine($"Error during export: {ex.Message}");
            }
        }
    }
}