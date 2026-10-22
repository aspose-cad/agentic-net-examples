using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using System.IO;

namespace CadTextQualityExample
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
        // Output raster image path (PNG format)
                    string outputPath = "output.png";

                    // Load the CAD drawing
                    using (Image cadImage = Image.Load(inputPath))
                    {
                        // Configure rasterization options
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                        rasterOptions.PageWidth = 1600;   // Width in pixels (adjust as needed)
                        rasterOptions.PageHeight = 1200;  // Height in pixels (adjust as needed)
                        rasterOptions.UnitType = UnitType.Millimeter;
                        rasterOptions.AutomaticLayoutsScaling = true;

                        // Set high quality for text, hatch and arc rendering
                        rasterOptions.Quality = new RasterizationQuality
                        {
                            Text = RasterizationQualityValue.High,
                            Hatch = RasterizationQualityValue.High,
                            Arc = RasterizationQualityValue.High
                        };

                        // Create PNG export options and assign rasterization settings
                        PngOptions pngOptions = new PngOptions();
                        pngOptions.VectorRasterizationOptions = rasterOptions;

                        // Save the CAD drawing as a high‑quality PNG image
                        cadImage.Save(outputPath, pngOptions);
                    }

                    Console.WriteLine("Export completed successfully.");
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
    }
}