using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input IGES file path
            string inputPath = "sample.iges";

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

            // Load the IGES file
            Image cadImage = Image.Load(inputPath);

            // Configure rasterization options.
            // Note: Aspose.CAD does not expose a direct AntiAliasing property.
            // Anti‑aliasing can be controlled via GraphicsOptions if supported by the underlying library,
            // but the current SDK version does not provide a specific setting for it.
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                PageWidth = 1600f,
                PageHeight = 1600f,
                GraphicsOptions = new GraphicsOptions()
            };

            // Set PDF export options and attach rasterization settings
            PdfOptions pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save the rendered PDF
            cadImage.Save(outputPath, pdfOptions);

            Console.WriteLine($"PDF successfully saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}