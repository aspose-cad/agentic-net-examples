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
            // Input STEP file and output PDF file paths
            string inputPath = "sample.stp";
            string outputPath = "output.pdf";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the STEP file using Aspose.CAD
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options (optional, can adjust page size, layers, layouts, etc.)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    // Layouts = null; // all layouts
                    // Layers = null; // all layers
                };

                // Set PDF export options and attach rasterization options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Export the CAD image to PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Export successful: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            // Print concise error information without crashing
            Console.Error.WriteLine($"Error during export: {ex.Message}");
        }
    }
}