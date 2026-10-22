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

            // Verify that the input CAD file exists
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

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options
                var rasterOptions = new CadRasterizationOptions
                {
                    // Example page size (in points) – A4
                    PageWidth = 595,
                    PageHeight = 842,
                    // Render all layouts (null or empty means all)
                    Layouts = null
                };

                // Set PDF export options and attach the rasterization settings
                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the drawing as PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"PDF file saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}