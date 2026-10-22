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
                string outputPath = "output.pdf";

                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                Image cadImage = Image.Load(inputPath);

                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 595,   // A4 width in points
                    PageHeight = 842,  // A4 height in points
                    UnitType = UnitType.Point,
                    LineScale = 1.0f,
                    Margins = new Margins
                    {
                        Left = 50,
                        Right = 50,
                        Top = 50,
                        Bottom = 50
                    }
                };

                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, pdfOptions);

                Console.WriteLine($"PDF saved to {outputPath}");
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}
