using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "sample.dwg";
        string outputPath = "output.pdf";

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Prepare PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Configure rasterization options if needed (e.g., page size, layouts, layers)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Example: set page size to A4
                    PageWidth = 595,
                    PageHeight = 842,
                    // Render all layouts and layers by default
                    // Layouts = new string[] { "Model" }, // uncomment to specify layouts
                    // Layers = new string[] { "0" }      // uncomment to specify layers
                };

                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save the CAD image as PDF
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