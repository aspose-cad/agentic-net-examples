using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class PdfAExportWithSmoothArcs
{
    static void Main()
    {
        // Input and output paths
        string inputFile = @"C:\CAD\sample.dwg";
        string outputFile = @"C:\Export\sample.pdf";

        // Verify input file exists
        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"Input file not found: {inputFile}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputFile);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputFile))
            {
                // Configure rasterization options for higher curve smoothness
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Use a smaller line scale to increase rendering precision (smooth arcs)
                    LineScale = 0.05f,

                    // Unit type for page dimensions (correct enum spelling)
                    UnitType = UnitType.Centimenter,

                    // Let the renderer calculate based on drawing size
                    AutomaticLayoutsScaling = true,

                    // Margins (all sides set to zero)
                    Margins = new Margins { Left = 0, Right = 0, Top = 0, Bottom = 0 },

                    // No specific layout filtering
                    Layouts = null
                };

                // Set up PDF options and attach the rasterization settings
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the drawing as PDF/A (Aspose.CAD uses PDF/A by default for PdfOptions)
                cadImage.Save(outputFile, pdfOptions);
            }

            Console.WriteLine($"Export completed successfully: {outputFile}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during export: {ex.Message}");
        }
    }
}