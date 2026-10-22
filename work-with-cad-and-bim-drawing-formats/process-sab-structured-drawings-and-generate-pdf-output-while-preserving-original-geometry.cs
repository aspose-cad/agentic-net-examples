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
            string inputPath = "input.dwg"; // placeholder for the source SAB structured drawing
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

            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options to preserve original geometry (vector output)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Default settings preserve geometry; customize if needed (e.g., Layouts, Layers)
                };

                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"PDF saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}