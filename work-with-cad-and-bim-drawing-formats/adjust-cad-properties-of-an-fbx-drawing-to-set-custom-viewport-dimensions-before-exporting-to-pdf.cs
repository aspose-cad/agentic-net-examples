using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using System.IO;

namespace CadExportExample
{
    class Program
    {
        static void Main()
        {
        try
        {
                    // Input FBX file path
                    string inputPath = "input.fbx";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Output PDF file path
                    string outputPath = "output.pdf";

                    // Load the CAD drawing
                    using (Image image = Image.Load(inputPath))
                    {
                        // Configure rasterization options for custom viewport
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                        // Set measurement unit (actual SDK spelling)
                        rasterOptions.UnitType = UnitType.Centimenter;
                        // Set line scaling factor
                        rasterOptions.LineScale = 1.0f;
                        // Define custom page (viewport) dimensions in centimeters
                        rasterOptions.PageWidth = 210.0f;   // Width
                        rasterOptions.PageHeight = 297.0f;  // Height
                        // Optional: set margins (in pixels)
                        Margins margins = new Margins();
                        margins.Left = 10;
                        margins.Right = 10;
                        margins.Top = 10;
                        margins.Bottom = 10;
                        rasterOptions.Margins = margins;

                        // Create PDF export options and assign rasterization settings
                        PdfOptions pdfOptions = new PdfOptions();
                        pdfOptions.VectorRasterizationOptions = rasterOptions;

                        // Save the drawing as PDF with the specified viewport settings
                        image.Save(outputPath, pdfOptions);
                    }
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
    }
}