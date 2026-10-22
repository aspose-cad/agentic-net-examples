using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadConversionExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file
                string sourcePath = "input.dwg";

                // Verify the source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Load the CAD drawing
                Image cadImage = Image.Load(sourcePath);

                // Configure rasterization options for vector formats (PDF, SVG, PNG)
                CadRasterizationOptions rasterizationOptions = new CadRasterizationOptions
                {
                    // UnitType line removed due to API version differences
                    LineScale = 1.0f,               // Keep original line thickness
                    PageWidth = 210f,               // A4 width in mm
                    PageHeight = 297f,              // A4 height in mm
                    NoScaling = true,               // Prevent automatic scaling
                    Margins = new Margins { Left = 10, Right = 10, Top = 10, Bottom = 10 }
                };

                // ---------- Export to PDF ----------
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterizationOptions
                };
                cadImage.Save("output.pdf", pdfOptions);

                // ---------- Export to SVG ----------
                SvgOptions svgOptions = new SvgOptions
                {
                    VectorRasterizationOptions = rasterizationOptions
                };
                cadImage.Save("output.svg", svgOptions);

                // ---------- Export to PNG (raster image) ----------
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterizationOptions
                };
                cadImage.Save("output.png", pngOptions);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}