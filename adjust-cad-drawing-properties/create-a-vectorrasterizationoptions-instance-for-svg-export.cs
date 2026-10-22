using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.svg";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image cadImage = Image.Load(inputPath))
            {
                // Create CadRasterizationOptions for SVG export (concrete implementation of VectorRasterizationOptions)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800,   // Width of the output page in pixels
                    PageHeight = 600,  // Height of the output page in pixels
                    UnitType = UnitType.Millimeter // Measurement unit for the output
                };

                // Configure SVG-specific options and assign the raster options
                SvgOptions svgOptions = new SvgOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                try
                {
                    // Ensure the output directory exists
                    string outputDir = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    // Save the drawing as SVG
                    cadImage.Save(outputPath, svgOptions);
                    Console.WriteLine($"SVG file saved successfully to {outputPath}");
                }
                catch (Exception saveEx)
                {
                    Console.WriteLine($"Error saving SVG file: {saveEx.Message}");
                }
            }
        }
        catch (Exception loadEx)
        {
            Console.WriteLine($"Error loading CAD file: {loadEx.Message}");
        }
    }
}
