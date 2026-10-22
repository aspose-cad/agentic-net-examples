using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
                // Path to the source CAD file (DWG, DXF, DGN, etc.)
                string inputPath = "input.dwg";

        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Load the CAD document
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Create PDF save options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Optional: control rasterization of vector data (page size, layout, etc.)
                    // pdfOptions.VectorRasterizationOptions = new CadRasterizationOptions
                    // {
                    //     PageWidth = 800,
                    //     PageHeight = 600,
                    //     Layouts = cadImage.Layouts.Values
                    // };

                    // Define output file path
                    string outputPath = "output.pdf";

                    // Save the CAD document as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}