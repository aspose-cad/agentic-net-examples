using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        // Input and output paths
        string inputPath = "input.dwg";
        string outputDir = "output";
        string outputPath = Path.Combine(outputDir, "result.pdf");

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine("Input file not found: " + inputPath);
            return;
        }

        // Ensure output directory exists
        Directory.CreateDirectory(outputDir);

        try
        {
            // Load CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Access layout names
                Console.WriteLine("Layouts:");
                foreach (CadLayout layout in cadImage.Layouts.Values)
                {
                    Console.WriteLine("- " + layout.LayoutName);
                }

                // NOTE: External references are not exposed in the current API version.
                // If needed, handle them via custom logic or a newer SDK version.

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Set unit type (spelling as per SDK)
                    UnitType = UnitType.Centimenter,
                    LineScale = 1.0f,
                    // Zero margins for a borderless PDF
                    Margins = new Margins { Left = 0, Right = 0, Top = 0, Bottom = 0 }
                };

                // Configure PDF export options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD image as a borderless PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine("PDF saved to: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}