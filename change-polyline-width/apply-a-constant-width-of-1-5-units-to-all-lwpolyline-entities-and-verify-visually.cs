using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                var cadImage = (CadImage)image;

                // Apply a constant line width of 1.5 units to all LWPOLYLINE entities
                foreach (var entity in cadImage.Entities)
                {
                    if (entity is CadLwPolyline lwPolyline)
                    {
                        // LineWeight is of type short; assign a value representing 1.5 units
                        lwPolyline.LineWeight = (short)150; // 1.5 * 100 (assuming 0.01 unit precision)
                    }
                }

                // Configure rasterization options for visual verification
                var rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,
                    LineScale = 1.0f,
                    PageWidth = 210f,   // A4 width in millimeters
                    PageHeight = 297f   // A4 height in millimeters
                };

                // Save the result to PDF to visually verify the line widths
                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}