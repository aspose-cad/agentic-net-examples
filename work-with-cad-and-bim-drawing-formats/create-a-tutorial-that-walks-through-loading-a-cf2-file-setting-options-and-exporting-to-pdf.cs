using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Placeholder paths – replace with actual file locations.
        string inputPath = "input.cf2";
        string outputPath = "output.pdf";

        try
        {
            // Verify that the source CF2 file exists.
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Configure PDF export options.
            PdfOptions pdfOptions = new PdfOptions();

            // Optional: control rasterization of vector data.
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                // Example settings – adjust as needed.
                PageWidth = 800,
                PageHeight = 600,
                // Set background color if desired (default is white).
                // BackgroundColor = System.Drawing.Color.White,
                // Preserve layers, layouts, etc., as required.
                // Layouts = new string[] { "Model" }
            };
            pdfOptions.VectorRasterizationOptions = rasterOptions;

            // Load the CF2 file.
            using (Image cadImage = Image.Load(inputPath))
            {
                // Export to PDF using the configured options.
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"Conversion completed successfully. PDF saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}