using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Input CAD file path
        string inputPath = "input.dwg";

        // Output directory for exported files
        string outputDir = "output";

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Prepare common rasterization options (used for vector formats)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // UnitType enum value is spelled "Centimenter" in the SDK
                    UnitType = UnitType.Centimenter,
                    LineScale = 1f,
                    PageWidth = 800,
                    PageHeight = 600,
                    // Export all model space layouts
                    Layouts = new[] { "*Model_Space" },
                    Margins = new Margins { Left = 10, Right = 10, Top = 10, Bottom = 10 }
                };

                // Export to PDF
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };
                string pdfPath = Path.Combine(outputDir, "drawing.pdf");
                cadImage.Save(pdfPath, pdfOptions);

                // Export to SVG
                SvgOptions svgOptions = new SvgOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };
                string svgPath = Path.Combine(outputDir, "drawing.svg");
                cadImage.Save(svgPath, svgOptions);

                // Export to PNG (raster image)
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };
                string pngPath = Path.Combine(outputDir, "drawing.png");
                cadImage.Save(pngPath, pngOptions);

                // Export to DXF (preserves native CAD entities, including hatches)
                DxfOptions dxfOptions = new DxfOptions();
                string dxfPath = Path.Combine(outputDir, "drawing.dxf");
                cadImage.Save(dxfPath, dxfOptions);

                Console.WriteLine("Export completed successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during processing: {ex.Message}");
        }
    }
}