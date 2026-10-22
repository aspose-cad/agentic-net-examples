using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dwg";
            string pdfOutputPath = "output.pdf";
            
            string outputDir = Path.GetDirectoryName(pdfOutputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
string pngOutputPath = "output.png";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Export to PDF (vector format)
                    PdfOptions pdfOptions = new PdfOptions();
                    CadRasterizationOptions pdfRasterOptions = new CadRasterizationOptions
                    {
                        // Optional: set page size or other rasterization settings
                        PageWidth = 800,
                        PageHeight = 600,
                        Layouts = null, // export all layouts
                        Layers = null   // export all layers
                    };
                    pdfOptions.VectorRasterizationOptions = pdfRasterOptions;
                    cadImage.Save(pdfOutputPath, pdfOptions);

                    // Export to PNG (raster format)
                    PngOptions pngOptions = new PngOptions();
                    CadRasterizationOptions pngRasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 800,
                        PageHeight = 600,
                        Layouts = null,
                        Layers = null
                    };
                    pngOptions.VectorRasterizationOptions = pngRasterOptions;
                    cadImage.Save(pngOutputPath, pngOptions);
                }

                Console.WriteLine("Export completed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}