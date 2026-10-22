using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Placeholder paths – replace with actual file locations
        string inputPath = "sample.dwg";
        string outputPath = "output.pdf";

        try
        {
            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Cast to CadImage if CAD-specific properties are needed
                CadImage cad = cadImage as CadImage;

                // Configure PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Optional rasterization settings (page size, layout, etc.)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1200,
                    PageHeight = 800,
                    // Example: export only the "Model" layout; adjust as needed
                    Layouts = new string[] { "Model" }
                };

                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save the drawing as PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"CAD file successfully saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            // Output concise error information
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
