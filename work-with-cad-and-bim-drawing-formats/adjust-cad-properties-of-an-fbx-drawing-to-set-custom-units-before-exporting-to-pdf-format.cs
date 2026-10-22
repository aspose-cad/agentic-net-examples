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
                    using (Image cadImage = Image.Load(inputPath))
                    {
                        // Create rasterization options for vector export
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions();

                        // Set custom unit type (Centimeter) and line scale
                        rasterOptions.UnitType = UnitType.Centimenter; // SDK spelling
                        rasterOptions.LineScale = 1.0f; // Adjust as needed

                        // Optional: configure page size, margins, etc.
                        // rasterOptions.PageWidth = 210f;   // A4 width in mm
                        // rasterOptions.PageHeight = 297f;  // A4 height in mm
                        // rasterOptions.Margins = new Margins { Left = 10, Right = 10, Top = 10, Bottom = 10 };

                        // Create PDF export options and assign rasterization settings
                        PdfOptions pdfOptions = new PdfOptions();
                        pdfOptions.VectorRasterizationOptions = rasterOptions;

                        // Save the drawing as PDF with the specified options
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