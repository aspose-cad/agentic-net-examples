using Aspose.CAD.FileFormats.Cad;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Input CAD file path (replace with actual path)
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            using (Image loadedImage = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD‑specific members
                var cadImage = loadedImage as Aspose.CAD.FileFormats.Cad.CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // NOTE: External reference handling is not available in the current Aspose.CAD API.
                // If needed, external references must be managed outside of this library.

                // Prepare output directory
                string outputDir = "output";
                Directory.CreateDirectory(outputDir);

                // Output PDF file path
                string outputPath = Path.Combine(outputDir, "drawing.pdf");

                // Set PDF export options
                var pdfOptions = new PdfOptions();

                // Save the drawing as PDF
                cadImage.Save(outputPath, pdfOptions);

                Console.WriteLine($"PDF exported successfully to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}