using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Path to the source IGES file
        string inputPath = "sample.igs";

        // Desired output PDF file path
        string outputPath = "output.pdf";

        try
        {
            // Verify that the input file exists
            if (!File.Exists(inputPath))
                throw new FileNotFoundException($"Input file not found: {inputPath}");

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Load the CAD image (IGES)
            using (Image cadImage = Image.Load(inputPath))
            {
                // Set up rasterization options with a light gray background
                var rasterOptions = new CadRasterizationOptions
                {
                    // Light gray (RGB 211,211,211)
                    BackgroundColor = Color.FromArgb(211, 211, 211),

                    // Optional: match page size to the image dimensions
                    PageWidth = cadImage.Width,
                    PageHeight = cadImage.Height
                };

                // Configure PDF export options
                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Render and save as PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine("IGES file rendered to PDF successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}