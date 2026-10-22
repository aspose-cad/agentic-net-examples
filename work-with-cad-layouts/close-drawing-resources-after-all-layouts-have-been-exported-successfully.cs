using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options (e.g., select a layout)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Example: render only the "Model" layout if it exists
                    Layouts = new string[] { "Model" },
                    // Optional: set page size or other rasterization settings here
                    // PageWidth = 800,
                    // PageHeight = 600,
                };

                // Set up PDF export options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the drawing as PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Successfully exported to PDF: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}