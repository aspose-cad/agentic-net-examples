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
            // Define input and output file paths
            string inputPath = "sample.dwg";
            string outputPath = "output.pdf";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options (page size, layout selection, etc.)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Example page size; adjust as needed
                    PageWidth = 1600,
                    PageHeight = 1200,
                    // Specify layout names if the CAD file contains multiple layouts
                    Layouts = new string[] { "Model" }
                };

                // Set up PDF export options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD image as PDF
                cadImage.Save(outputPath, pdfOptions);

                // Log layout size information after successful export
                Console.WriteLine($"Export succeeded. Layout size: {rasterOptions.PageWidth} x {rasterOptions.PageHeight} points.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error during CAD to PDF export: {ex.Message}");
        }
    }
}