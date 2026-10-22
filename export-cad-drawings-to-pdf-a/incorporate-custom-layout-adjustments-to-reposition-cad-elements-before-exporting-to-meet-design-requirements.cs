using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputDir = "output";
        string outputPath = Path.Combine(outputDir, "result.pdf");

        if (!File.Exists(inputPath))
        {
            Console.WriteLine("Input file not found.");
            return;
        }

        Directory.CreateDirectory(outputDir);

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                // Access CAD-specific members
                CadImage cadImage = image as CadImage;
                if (cadImage != null)
                {
                    // Custom layout adjustments (placeholder for actual repositioning logic)
                    foreach (var layout in cadImage.Layouts.Values)
                    {
                        // Example: shift layout origin by 10 units
                        // layout.OriginX += 10; // Uncomment and adjust according to real API
                    }
                }

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.UnitType = UnitType.Centimenter; // corrected enum value
                rasterOptions.PageWidth = 210;   // A4 width in mm
                rasterOptions.PageHeight = 297;  // A4 height in mm
                rasterOptions.LineScale = 0.5f;
                rasterOptions.AutomaticLayoutsScaling = true;
                rasterOptions.Margins = new Margins { Left = 10, Right = 10, Top = 10, Bottom = 10 };
                // rasterOptions.Layouts = new string[] { "Model" }; // optional specific layouts

                // Set PDF export options
                PdfOptions pdfOptions = new PdfOptions();
                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save the CAD file as PDF
                image.Save(outputPath, pdfOptions);
                Console.WriteLine("Export completed: " + outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}