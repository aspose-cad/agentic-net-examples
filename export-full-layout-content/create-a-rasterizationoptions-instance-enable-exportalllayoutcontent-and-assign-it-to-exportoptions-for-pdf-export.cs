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
            // Define input and output paths
            string inputPath = "input.dwg";
            string outputDir = "Output";
            string outputPath = Path.Combine(outputDir, "output.pdf");

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create rasterization options and enable ExportAllLayoutContent
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
            rasterOptions.ExportAllLayoutContent = true;

            // Create PDF export options and assign the rasterization options
            PdfOptions pdfOptions = new PdfOptions();
            pdfOptions.VectorRasterizationOptions = rasterOptions;

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Save the drawing as PDF using the configured options
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"PDF file saved successfully to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}