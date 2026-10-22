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
                    // Path to the source CAD drawing
                    string inputPath = "input.dwg";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Path for the exported PDF
                    string outputPath = "output.pdf";

                    // Load the CAD drawing
                    using (Image image = Image.Load(inputPath))
                    {
                        // Cast to CadImage to access CAD-specific members
                        CadImage cadImage = (CadImage)image;

                        // Configure rasterization options for PDF export
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                        // Set measurement unit (Centimeter)
                        rasterOptions.UnitType = UnitType.Centimenter;
                        // Set line scaling factor
                        rasterOptions.LineScale = 1.0f;
                        // Define page size (width and height in centimeters)
                        rasterOptions.PageWidth = 21.0f;   // A4 width
                        rasterOptions.PageHeight = 29.7f; // A4 height
                        // Add top margin to create space for a title block
                        rasterOptions.Margins = new Margins
                        {
                            Top = 30,    // 30 cm margin at the top for the title block
                            Bottom = 0,
                            Left = 0,
                            Right = 0
                        };
                        // Specify which layout(s) to render; using the default layout name
                        rasterOptions.Layouts = new string[] { "Model" };
                        // Optional: automatic scaling of layouts
                        rasterOptions.AutomaticLayoutsScaling = true;

                        // Create PDF export options and assign the rasterization settings
                        PdfOptions pdfOptions = new PdfOptions();
                        pdfOptions.VectorRasterizationOptions = rasterOptions;

                        // Export the CAD drawing to PDF
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
}