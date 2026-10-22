using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class CadMarginRemoval
{
    static void Main()
    {
        // Input CAD file path
        string inputPath = @"C:\Input\sample.dwg";

        
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }
// Output image file path (PNG used for verification)
        string outputPath = @"C:\Output\sample_no_margin.png";

        // Ensure the output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD drawing as a concrete CadImage instance
            CadImage cadImage = (CadImage)Image.Load(inputPath);

            // Configure rasterization options: set Zoom to 1 and remove margins
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                Zoom = 1f,
                // The SDK uses the misspelled enum value "Centimenter"
                UnitType = UnitType.Centimenter,
                // Set all margins to zero to eliminate surrounding whitespace
                Margins = new Margins
                {
                    Left = 0,
                    Right = 0,
                    Top = 0,
                    Bottom = 0
                }
            };

            // Create PNG export options and assign the rasterization settings
            PngOptions pngOptions = new PngOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save the image; the resulting PNG should have no extra margins
            cadImage.Save(outputPath, pngOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}