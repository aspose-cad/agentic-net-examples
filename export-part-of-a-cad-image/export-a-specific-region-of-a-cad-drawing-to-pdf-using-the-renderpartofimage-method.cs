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

            string outputDirectory = "output";
            Directory.CreateDirectory(outputDirectory);
            string outputPath = Path.Combine(outputDirectory, "region.pdf");

            // Define the region to export (in drawing units, e.g., millimeters)
            float regionX = 0f;
            float regionY = 0f;
            float regionWidth = 100f;
            float regionHeight = 100f;

            using (Image cadImage = Image.Load(inputPath))
            {
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = regionWidth,
                    PageHeight = regionHeight,
                    UnitType = UnitType.Millimeter,
                    LineScale = 1.0f,
                    AutomaticLayoutsScaling = false,
                    Margins = new Margins
                    {
                        Left = (int)-regionX,
                        Top = (int)-regionY,
                        Right = 0,
                        Bottom = 0
                    }
                };

                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"Region exported successfully to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}