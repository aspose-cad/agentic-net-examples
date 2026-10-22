using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input CAD file path (DWG/DXF)
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Output PDF file path
            string outputPath = "output.pdf";

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Unable to load CAD image.");
                    return;
                }

                // Configure PDF export options with margins and line scaling
                var rasterOptions = new CadRasterizationOptions
                {
                    // Set unit type to centimeters (actual SDK spelling)
                    UnitType = UnitType.Centimenter,
                    // A4 dimensions: 21 cm x 29.7 cm
                    PageWidth = 21f,
                    PageHeight = 29.7f,
                    // Scale lines to achieve desired visual weight
                    LineScale = 0.25f,
                    // Define 1 cm margins on each side
                    Margins = new Margins
                    {
                        Top = 1,
                        Bottom = 1,
                        Left = 1,
                        Right = 1
                    }
                };

                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the modified drawing as PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine("PDF file saved to " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}
