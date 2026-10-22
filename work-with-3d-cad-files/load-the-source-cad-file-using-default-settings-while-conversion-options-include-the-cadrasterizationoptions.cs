using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.pdf";

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file with default settings
            using (Image cadImage = Image.Load(inputPath))
            {
                // Prepare rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Example settings – adjust as needed
                    PageWidth = 1600,
                    PageHeight = 1200,
                    // Render all layouts; specify names if required
                    // Layouts = new string[] { "Model" }
                };

                // Prepare PDF export options and attach rasterization options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD file as PDF using the specified options
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"File saved successfully to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}