using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class ExportSpecificLayout
{
    static void Main()
    {
        const string inputPath = "input.dxf";
        const string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the source DXF file
            CadImage cadImage = (CadImage)Image.Load(inputPath);

            // Choose the layout you want to export (e.g., the first layout)
            string layoutName = cadImage.Layouts.Keys.FirstOrDefault();
            if (string.IsNullOrEmpty(layoutName))
            {
                Console.Error.WriteLine("No layouts found in the DXF file.");
                return;
            }

            // Configure rasterization options to preserve layout dimensions
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                UnitType = UnitType.Centimenter,
                LineScale = 1.0f,
                AutomaticLayoutsScaling = false,
                Layouts = new[] { layoutName },
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

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Save the selected layout as an image
            cadImage.Save(outputPath, pngOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}