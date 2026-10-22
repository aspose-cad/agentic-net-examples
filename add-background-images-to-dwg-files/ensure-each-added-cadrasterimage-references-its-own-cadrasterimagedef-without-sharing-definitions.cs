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
            // Input CAD file path (DWG, DXF, etc.)
            string inputPath = "input.dwg";

            // Output file paths
            string pdfOutputPath = "output.pdf";
            
            string outputDir = Path.GetDirectoryName(pdfOutputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
string svgOutputPath = "output.svg";
            string pngOutputPath = "output.png";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Export to PDF
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(pdfOutputPath, pdfOptions);

                // Export to SVG
                SvgOptions svgOptions = new SvgOptions();
                cadImage.Save(svgOutputPath, svgOptions);

                // Export to PNG with rasterization options
                PngOptions pngOptions = new PngOptions();
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.PageWidth = 1600;   // Desired width in pixels
                rasterOptions.PageHeight = 1200;  // Desired height in pixels
                pngOptions.VectorRasterizationOptions = rasterOptions;
                cadImage.Save(pngOutputPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
