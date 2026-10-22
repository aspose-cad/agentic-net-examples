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
            // Placeholder paths – replace with actual file locations
            string inputPath = "sample.dwg";
            string outputPath = "output.pdf";

            // Verify that the input CAD file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file using Aspose.CAD
            using (Image cadImage = Image.Load(inputPath))
            {
                // Set up PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // If specific rasterization settings are needed, configure them here:
                // pdfOptions.VectorRasterizationOptions = new CadRasterizationOptions();

                // Save the CAD drawing as a PDF file
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"CAD file successfully saved as PDF to: {outputPath}");
        }
        catch (Exception ex)
        {
            // Print concise error information without crashing
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}