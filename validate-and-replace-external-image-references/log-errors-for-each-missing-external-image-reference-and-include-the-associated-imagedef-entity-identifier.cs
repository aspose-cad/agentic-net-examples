using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Placeholder paths – replace with actual file locations
        string inputPath = "sample.dwg";
        string outputPath = "output.pdf";

        try
        {
            // Verify that the input CAD file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options for vector export
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.PageWidth = 1600;   // Desired output width in pixels
                rasterOptions.PageHeight = 1200;  // Desired output height in pixels
                // rasterOptions.Layouts = new string[] { "Model" }; // Example layout selection
                // rasterOptions.Layers = new string[] { "0" };       // Example layer selection

                // Set up PDF export options
                PdfOptions pdfOptions = new PdfOptions();
                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save the drawing as PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"File saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            // Log any errors without crashing the application
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}