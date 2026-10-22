using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using System.IO;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Input CAD file path
                    string inputPath = "input.dwg";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Output PDF file path
                    string outputPath = "output.pdf";

                    // Load the CAD image
                    using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                    {
                        // Configure rasterization options
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                        rasterOptions.UnitType = UnitType.Centimenter; // Set unit type
                        rasterOptions.LineScale = 1.0f;                // Set line scale

                        // Optional: set margins
                        rasterOptions.Margins = new Margins();
                        rasterOptions.Margins.Left = 10;
                        rasterOptions.Margins.Right = 10;
                        rasterOptions.Margins.Top = 10;
                        rasterOptions.Margins.Bottom = 10;

                        // Create PDF export options and assign rasterization settings
                        PdfOptions pdfOptions = new PdfOptions();
                        pdfOptions.VectorRasterizationOptions = rasterOptions;

                        // Save the CAD drawing as PDF
                        cadImage.Save(outputPath, pdfOptions);
                    }
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
    }
}