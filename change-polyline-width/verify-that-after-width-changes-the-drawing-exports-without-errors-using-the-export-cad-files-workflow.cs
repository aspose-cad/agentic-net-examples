using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using System.IO;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Path to the source CAD file
                    string inputPath = "input.dwg";

            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Path for the exported PDF file
                    string outputPath = "output.pdf";

                    // Load the CAD file
                    using (Image loadedImage = Image.Load(inputPath))
                    {
                        // Cast to CadImage to access CAD-specific members if needed
                        CadImage cadImage = loadedImage as CadImage;

                        // Configure rasterization options to control output dimensions (width change)
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                        rasterOptions.PageWidth = 2000;   // Desired width in pixels
                        rasterOptions.PageHeight = 2000;  // Desired height in pixels (adjust as needed)

                        // Set up PDF export options using the rasterization settings
                        PdfOptions pdfOptions = new PdfOptions();
                        pdfOptions.VectorRasterizationOptions = rasterOptions;

                        // Export the CAD drawing to PDF
                        cadImage.Save(outputPath, pdfOptions);
                    }

                    Console.WriteLine("Export completed successfully.");
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
    }
}