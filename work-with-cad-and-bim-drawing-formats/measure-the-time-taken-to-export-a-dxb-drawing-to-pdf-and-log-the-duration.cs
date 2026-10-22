using System;
using System.IO;
using System.Diagnostics;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Define input and output file paths
        string inputPath = "input.dxb";
        string outputPath = "output.pdf";

        // Verify that the input file exists
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Start timing the export operation
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Load the DXB drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Configure rasterization options for vector output
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Example page size; adjust as needed
                    PageWidth = 1600,
                    PageHeight = 1200
                };

                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save the drawing as PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            // Stop timing and report duration
            stopwatch.Stop();
            Console.WriteLine($"Export completed in {stopwatch.Elapsed.TotalSeconds:F2} seconds.");
        }
        catch (Exception ex)
        {
            // Log any errors without crashing
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}