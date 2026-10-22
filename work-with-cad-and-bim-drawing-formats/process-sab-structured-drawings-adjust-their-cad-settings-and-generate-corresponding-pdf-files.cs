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
            string inputPath = "input.dwg";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDirectory = "output";
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            string outputPath = Path.Combine(outputDirectory, "output.pdf");

            // Load CAD image
            Image cadImage = Image.Load(inputPath);

            // Set rasterization options
            CadRasterizationOptions rasterizationOptions = new CadRasterizationOptions
            {
                PageWidth = 1200,
                PageHeight = 800,
                Layouts = new string[] { "Model" }
            };

            // Set PDF options
            PdfOptions pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterizationOptions
            };

            // Save as PDF
            cadImage.Save(outputPath, pdfOptions);

            Console.WriteLine($"PDF saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}