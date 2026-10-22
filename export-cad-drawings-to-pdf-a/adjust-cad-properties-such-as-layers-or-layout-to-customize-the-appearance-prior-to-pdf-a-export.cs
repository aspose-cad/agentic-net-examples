using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
            return;
        }

        try
        {
            using (Image cadImage = Image.Load(inputPath))
            {
                CadImage cad = cadImage as CadImage;
                if (cad == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Configure rasterization options to control layers and layout
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layers = new[] { "VisibleLayer" },   // replace with actual layer names
                    Layouts = new[] { "Layout1" }        // replace with actual layout name
                };

                // Set up PDF export options (PDF/A compliance can be set if supported)
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                    // Compliance = PdfCompliance.PdfA1b // uncomment if property exists
                };

                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Export completed: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}