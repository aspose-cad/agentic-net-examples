using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Paths
        string inputPath = "sample.iges";
        string outputDir = "output";
        string outputPath = Path.Combine(outputDir, "sample.pdf");

        try
        {
            // Verify input file exists
            if (!File.Exists(inputPath))
                throw new FileNotFoundException("Input file not found.", inputPath);

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Load the IGES file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Set up rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Black background
                    BackgroundColor = Color.Black,
                    // Unit type (centimeter)
                    UnitType = UnitType.Centimenter,
                    // Adjust line thickness scaling
                    LineScale = 0.5f,
                    // Page size based on image dimensions
                    PageWidth = cadImage.Width,
                    PageHeight = cadImage.Height
                };

                // Configure PDF export options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save as PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine("IGES file successfully rendered to PDF.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}