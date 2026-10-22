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
            // Input CAD file path
            string inputPath = "sample.dwg";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Create output directory
            string outputDir = "Output";
            Directory.CreateDirectory(outputDir);

            // Load CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Export to PDF
                string pdfPath = Path.Combine(outputDir, "output.pdf");
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(pdfPath, pdfOptions);

                // Export to SVG
                string svgPath = Path.Combine(outputDir, "output.svg");
                SvgOptions svgOptions = new SvgOptions();
                cadImage.Save(svgPath, svgOptions);

                // Export to PNG with layout selection
                string pngPath = Path.Combine(outputDir, "output.png");
                PngOptions pngOptions = new PngOptions();
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.Layouts = new string[] { "Model" };
                pngOptions.VectorRasterizationOptions = rasterOptions;
                cadImage.Save(pngPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}