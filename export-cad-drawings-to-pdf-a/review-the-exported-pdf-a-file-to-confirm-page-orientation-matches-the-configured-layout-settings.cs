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
            // Define input CAD file path
            string inputPath = "input.dwg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare output directory and file path
            string outputDir = "Output";
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, "output.pdf");

            // Load the CAD drawing
            Image cadImage = Image.Load(inputPath);

            // Configure rasterization options for layout and page size
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                // Specify the layout name to render (adjust as needed)
                Layouts = new string[] { "Model" },

                // Set page size to A4 portrait (points: 595 x 842)
                PageWidth = 595,
                PageHeight = 842
            };

            // Configure PDF export options
            PdfOptions pdfOptions = new PdfOptions
            {
                // Attach the rasterization settings
                VectorRasterizationOptions = rasterOptions
            };

            // Save the drawing as PDF
            cadImage.Save(outputPath, pdfOptions);

            // At this point the PDF file is created.
            // Review the file manually (or with a PDF viewer) to confirm that the page orientation matches the configured layout settings.
            Console.WriteLine($"PDF file saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}