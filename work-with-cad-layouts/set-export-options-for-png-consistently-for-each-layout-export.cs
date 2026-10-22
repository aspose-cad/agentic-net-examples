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

        try
        {
            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Cast to CadImage to access layout information
                CadImage cad = cadImage as CadImage;
                if (cad == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // If there are no layouts, export the whole drawing as a single PNG
                if (cad.Layouts == null || cad.Layouts.Count == 0)
                {
                    ExportLayout(cad, null, outputDir);
                }
                else
                {
                    // Export each layout separately
                    foreach (string layoutName in cad.Layouts.Keys)
                    {
                        ExportLayout(cad, layoutName, outputDir);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Exports a specific layout (or the whole drawing if layoutName is null) to PNG
    private static void ExportLayout(CadImage cadImage, string layoutName, string outputDir)
    {
        // Configure rasterization options
        CadRasterizationOptions rasterOptions = new CadRasterizationOptions
        {
            // Consistent output size for all PNG exports (units are millimeters by default)
            UnitType = UnitType.Millimeter,
            PageWidth = 1600,
            PageHeight = 1200,
            // Set the layout to render; if null, render the default view
            Layouts = layoutName != null ? new string[] { layoutName } : null,
            AutomaticLayoutsScaling = false
        };

        // Configure PNG export options
        PngOptions pngOptions = new PngOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        // Determine output file name
        string fileName = layoutName != null ? $"{layoutName}.png" : "full_drawing.png";
        string outputPath = Path.Combine(outputDir, fileName);

        // Save the PNG
        cadImage.Save(outputPath, pngOptions);
        Console.WriteLine($"Exported {(layoutName ?? "full drawing")} to {outputPath}");
    }
}