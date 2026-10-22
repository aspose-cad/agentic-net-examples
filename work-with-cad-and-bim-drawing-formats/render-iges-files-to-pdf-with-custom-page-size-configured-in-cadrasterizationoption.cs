using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class Program
{
    static void Main()
    {
        // Placeholder paths – replace with actual file locations.
        string inputPath = "sample.iges";
        string outputPath = "output.pdf";

        // Verify that the input file exists.
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the IGES file.
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options with a custom page size (Letter: 8.5\" x 11\").
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Page size in points (1 point = 1/72 inch).
                    PageWidth = 8.5f * 72f,
                    PageHeight = 11f * 72f,
                    // Optional: set background color, layout, etc., if needed.
                    // BackgroundColor = System.Drawing.Color.White,
                    // Layouts = new string[] { "Model" }
                };

                // Set up PDF export options using the rasterization settings.
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the rendered PDF.
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"PDF successfully saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}