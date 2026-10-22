using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            // Define input CAD file path
            string inputPath = "input.dwg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Create output directory
            string outputDir = "output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD image
            using (Image cadImage = Image.Load(inputPath))
            {
                // ---------- Export to PDF ----------
                string pdfPath = Path.Combine(outputDir, "output.pdf");
                PdfOptions pdfOptions = new PdfOptions();
                // Use vector rasterization options for PDF
                pdfOptions.VectorRasterizationOptions = new CadRasterizationOptions()
                {
                    // Example: set page size to A4
                    PageWidth = 595,
                    PageHeight = 842,
                    // Render all layouts
                    Layouts = new string[] { "Model" }
                };
                cadImage.Save(pdfPath, pdfOptions);
                Console.WriteLine($"Saved PDF to {pdfPath}");

                // ---------- Export to SVG ----------
                string svgPath = Path.Combine(outputDir, "output.svg");
                SvgOptions svgOptions = new SvgOptions();
                svgOptions.VectorRasterizationOptions = new CadRasterizationOptions()
                {
                    // Example: set size for SVG output
                    PageWidth = 800,
                    PageHeight = 600,
                    Layouts = new string[] { "Model" }
                };
                cadImage.Save(svgPath, svgOptions);
                Console.WriteLine($"Saved SVG to {svgPath}");

                // ---------- Export to PNG ----------
                string pngPath = Path.Combine(outputDir, "output.png");
                PngOptions pngOptions = new PngOptions();
                // Rasterization options for PNG
                pngOptions.VectorRasterizationOptions = new CadRasterizationOptions()
                {
                    // Set raster image size
                    PageWidth = 1200,
                    PageHeight = 800,
                    // Optionally select specific layers
                    // Layers = new string[] { "Layer1", "Layer2" },
                    Layouts = new string[] { "Model" },
                    // Quality settings affect raster output
                    // For PNG, Quality is not applicable; use default compression
                };
                cadImage.Save(pngPath, pngOptions);
                Console.WriteLine($"Saved PNG to {pngPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}