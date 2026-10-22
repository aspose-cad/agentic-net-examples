using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Input and output paths
        string inputPath = "input.dwg";
        string outputPath = "output.png";

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load CAD image
            CadImage cadImage = (CadImage)Image.Load(inputPath);

            // Configure rasterization options
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                UnitType = UnitType.Centimenter, // actual enum spelling
                LineScale = 0.5f,
                PageWidth = 210,   // A4 width in mm (interpreted by UnitType)
                PageHeight = 297,  // A4 height in mm
                AutomaticLayoutsScaling = true,
                Margins = new Margins
                {
                    Left = 10,
                    Right = 10,
                    Top = 10,
                    Bottom = 10
                }
            };

            // Prepare PNG export options
            PngOptions pngOptions = new PngOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save rendered image
            cadImage.Save(outputPath, pngOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}