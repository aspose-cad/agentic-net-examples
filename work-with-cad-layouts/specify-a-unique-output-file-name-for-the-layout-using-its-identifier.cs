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
            string inputFilePath = "input.dwg";

            if (!File.Exists(inputFilePath))
            {
                Console.Error.WriteLine($"Input file not found: {inputFilePath}");
                return;
            }

            string outputDir = "output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (CadImage cadImage = (CadImage)Image.Load(inputFilePath))
            {
                foreach (var layoutKvp in cadImage.Layouts)
                {
                    string layoutId = layoutKvp.Key;

                    // Export to PDF for this layout
                    string pdfOutputPath = Path.Combine(outputDir, $"{layoutId}_output.pdf");
                    var pdfOptions = new PdfOptions();
                    var pdfRasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 595,
                        PageHeight = 842,
                        Layouts = new[] { layoutId },
                        Layers = null
                    };
                    pdfOptions.VectorRasterizationOptions = pdfRasterOptions;
                    cadImage.Save(pdfOutputPath, pdfOptions);
                    Console.WriteLine($"PDF saved for layout {layoutId} to: {pdfOutputPath}");

                    // Export to PNG for this layout
                    string pngOutputPath = Path.Combine(outputDir, $"{layoutId}_output.png");
                    var pngOptions = new PngOptions();
                    var pngRasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200,
                        Layouts = new[] { layoutId },
                        Layers = null
                    };
                    pngOptions.VectorRasterizationOptions = pngRasterOptions;
                    cadImage.Save(pngOutputPath, pngOptions);
                    Console.WriteLine($"PNG saved for layout {layoutId} to: {pngOutputPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}