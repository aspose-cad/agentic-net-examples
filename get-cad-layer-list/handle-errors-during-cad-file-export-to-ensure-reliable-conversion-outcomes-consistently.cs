using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Placeholder paths – replace with actual file locations
        string inputPath = "input.dwg";
        string outputPath = "output.pdf";

        // Verify that the input file exists
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD file
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD‑specific features
                CadImage cadImage = image as CadImage;

                // Configure rasterization options (optional – adjust as needed)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200
                    // Layouts = new string[] { "Model" },
                    // Layers = new string[] { "0" }
                };

                // Set up PDF export options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD file as PDF
                if (cadImage != null)
                {
                    cadImage.Save(outputPath, pdfOptions);
                }
                else
                {
                    image.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Conversion succeeded: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}