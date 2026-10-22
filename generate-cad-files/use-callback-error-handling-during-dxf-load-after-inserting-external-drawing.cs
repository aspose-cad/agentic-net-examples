using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

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
                // Export to PDF
                string pdfPath = Path.Combine(outputDir, "output.pdf");
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(pdfPath, pdfOptions);
                Console.WriteLine($"Saved PDF to {pdfPath}");

                // Export to SVG
                string svgPath = Path.Combine(outputDir, "output.svg");
                SvgOptions svgOptions = new SvgOptions();
                cadImage.Save(svgPath, svgOptions);
                Console.WriteLine($"Saved SVG to {svgPath}");

                // Export to PNG (raster image)
                string pngPath = Path.Combine(outputDir, "output.png");
                PngOptions pngOptions = new PngOptions();
                // Optional: control rasterization size
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    // You can set Layouts or Layers here if needed
                };
                pngOptions.VectorRasterizationOptions = rasterOptions;
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