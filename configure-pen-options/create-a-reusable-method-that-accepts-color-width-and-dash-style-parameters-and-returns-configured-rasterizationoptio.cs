using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadPenOptionsDemo
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define source CAD file.
                string sourcePath = Path.Combine(Environment.CurrentDirectory, "input.dwg");
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Prepare output directory.
                string outputDir = Path.Combine(Environment.CurrentDirectory, "output");
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the CAD drawing.
                using (Image image = Image.Load(sourcePath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // Configure pen options (color, width, dash style).
                    string penColor = "Red";          // Placeholder, not used directly.
                    double penWidth = 0.5;            // Width in millimetres.
                    string dashStyle = "Dash";        // Placeholder, not used directly.

                    CadRasterizationOptions rasterOptions = CreateRasterizationOptions(penColor, penWidth, dashStyle);

                    // Export to PDF.
                    string pdfPath = Path.Combine(outputDir, "result.pdf");
                    var pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };
                    cadImage.Save(pdfPath, pdfOptions);
                    Console.WriteLine($"PDF saved to: {pdfPath}");

                    // Export to PNG.
                    string pngPath = Path.Combine(outputDir, "result.png");
                    var pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };
                    cadImage.Save(pngPath, pngOptions);
                    Console.WriteLine($"PNG saved to: {pngPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }

        // Creates a CadRasterizationOptions instance configured with the supplied parameters.
        static CadRasterizationOptions CreateRasterizationOptions(string penColor, double penWidth, string dashStyle)
        {
            var rasterOptions = new CadRasterizationOptions
            {
                // Set unit type (centimeter spelling as per API).
                UnitType = UnitType.Centimenter,
                // LineScale influences line thickness; using penWidth as a factor.
                LineScale = (float)penWidth,
                // Default page size; can be adjusted as needed.
                PageWidth = 210,   // A4 width in millimetres.
                PageHeight = 297,  // A4 height in millimetres.
                AutomaticLayoutsScaling = true,
                Margins = new Margins { Left = 0, Right = 0, Top = 0, Bottom = 0 }
            };

            // Note: Specific pen colour and dash style are not directly supported
            // by the current CadRasterizationOptions API, so they are not applied here.

            return rasterOptions;
        }
    }
}