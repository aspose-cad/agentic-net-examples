using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output file paths
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

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

                // Adjust the default font to prevent text clipping or overlap
                // when the original font is unavailable in the environment
                cadImage.DefaultFont = "Arial";

                // Set up PDF export options (no special rasterization needed for font handling)
                PdfOptions pdfOptions = new PdfOptions();

                // Save the CAD drawing to PDF using the adjusted font settings
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"File saved successfully to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}