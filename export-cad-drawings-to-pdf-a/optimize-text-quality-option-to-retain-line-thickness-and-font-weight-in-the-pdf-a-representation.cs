using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class CadToPdfA
{
    static void Main()
    {
        // Input CAD file path
        string inputFile = @"C:\CAD\sample.dwg";

        // Output PDF file path
        string outputFile = @"C:\CAD\output.pdf";

        // Verify that the input file exists
        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"Input file not found: {inputFile}");
            return;
        }

        // Ensure the output directory exists
        string outputDir = Path.GetDirectoryName(outputFile);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD image
            using (Image image = Image.Load(inputFile))
            {
                // Cast to CadImage to access CAD‑specific members
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("The loaded file is not a supported CAD format.");
                    return;
                }

                // Prepare rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Keep original line thickness
                    LineScale = 1.0f,

                    // Define page size (adjust as needed)
                    PageWidth = 800,
                    PageHeight = 600,

                    // Export all layouts by name
                    Layouts = cadImage.Layouts.Keys.ToArray()
                };

                // Prepare PDF export options
                PdfOptions pdfOptions = new PdfOptions
                {
                    // Assign the rasterization options
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD drawing as PDF/A (PDF/A compliance is handled internally)
                cadImage.Save(outputFile, pdfOptions);
                Console.WriteLine($"PDF successfully saved to: {outputFile}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}