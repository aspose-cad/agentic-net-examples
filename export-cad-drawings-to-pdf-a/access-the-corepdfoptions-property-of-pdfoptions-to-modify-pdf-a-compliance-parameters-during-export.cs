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
            string inputPath = "input.dwg";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDir = "output";
            Directory.CreateDirectory(outputDir);

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Export to PDF
                string pdfPath = Path.Combine(outputDir, "output.pdf");
                PdfOptions pdfOptions = new PdfOptions();
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new string[] { "Model" }
                };
                pdfOptions.VectorRasterizationOptions = rasterOptions;
                cadImage.Save(pdfPath, pdfOptions);

                // Export to SVG
                string svgPath = Path.Combine(outputDir, "output.svg");
                SvgOptions svgOptions = new SvgOptions();
                cadImage.Save(svgPath, svgOptions);

                // Export to PNG
                string pngPath = Path.Combine(outputDir, "output.png");
                PngOptions pngOptions = new PngOptions();
                cadImage.Save(pngPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}