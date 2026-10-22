using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source CAD file
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Path for the resulting PDF file
            string outputPath = "output.pdf";

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("The loaded file is not a CAD image.");
                    return;
                }

                // Retrieve layout names
                List<string> layoutNames = new List<string>(cadImage.Layouts.Keys);

                // Select a layout (use the first one if available)
                string selectedLayout = layoutNames.Count > 0 ? layoutNames[0] : null;

                // Configure rasterization options for the selected layout and region
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layouts = selectedLayout != null ? new[] { selectedLayout } : null,
                    UnitType = UnitType.Millimeter,
                    PageWidth = 210f,   // A4 width in millimeters
                    PageHeight = 297f,  // A4 height in millimeters
                    Margins = new Margins { Left = 0, Right = 0, Top = 0, Bottom = 0 }
                };

                // Configure PDF options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the selected layout region as PDF
                image.Save(outputPath, pdfOptions);
                Console.WriteLine($"PDF saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}