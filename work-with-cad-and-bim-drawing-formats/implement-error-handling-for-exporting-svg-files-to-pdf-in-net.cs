using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using System.IO;

class Program
{
    static void Main()
    {
        string inputPath = "input.svg";
        
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }
string outputPath = "output.pdf";

        try
        {
            // Load the SVG file
            using (Image image = Image.Load(inputPath))
            {
                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.UnitType = UnitType.Centimenter;
                rasterOptions.LineScale = 1.0f;
                rasterOptions.PageWidth = 800;
                rasterOptions.PageHeight = 600;
                rasterOptions.Margins = new Margins
                {
                    Left = 10,
                    Right = 10,
                    Top = 10,
                    Bottom = 10
                };

                // Set up PDF export options
                PdfOptions pdfOptions = new PdfOptions();
                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save the image as PDF
                image.Save(outputPath, pdfOptions);
            }

            Console.WriteLine("SVG exported to PDF successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during SVG to PDF export: {ex.Message}");
        }
    }
}