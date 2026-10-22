using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Define input and output file paths
        string inputPath = "input.dwg";
        string outputPath = "output.pdf";

        // Simple logger method
        void Log(string message) => Console.WriteLine($"{DateTime.Now:O} - {message}");

        try
        {
            Log("AutoCAD conversion started.");

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Log($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file using Aspose.CAD
            using (Image cadImage = Image.Load(inputPath))
            {
                Log($"Loaded CAD file: {inputPath}");

                // Configure PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Configure rasterization options if needed (e.g., page size)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Example page size (A4 in points)
                    PageWidth = 595,
                    PageHeight = 842
                    // Layers and Layouts can be set here if required
                };
                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save the CAD image as PDF
                cadImage.Save(outputPath, pdfOptions);
                Log($"Saved PDF file: {outputPath}");
            }

            Log("AutoCAD conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Log($"Error during conversion: {ex.Message}");
        }
    }
}