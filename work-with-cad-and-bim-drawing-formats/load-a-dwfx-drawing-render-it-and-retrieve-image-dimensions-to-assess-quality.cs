using System;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
                // Paths to the source DWFx file and the rendered PNG output
                string inputPath = "drawing.dwf";
        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        string outputPath = "rendered.png";

                // Load the DWFx drawing
                using (Image cadImage = CadImage.Load(inputPath))
                {
                    // Configure rasterization options
                    var raster = new CadRasterizationOptions
                    {
                        UnitType = UnitType.Centimenter,   // SDK spelling
                        LineScale = 1.0f,                  // Adjust as needed
                        Quality = new RasterizationQuality
                        {
                            Text = RasterizationQualityValue.High,
                            Hatch = RasterizationQualityValue.High,
                            Arc = RasterizationQualityValue.High
                        }
                    };

                    // Set up PNG export options and assign rasterization settings
                    var pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = raster
                    };

                    // Render the drawing to PNG
                    cadImage.Save(outputPath, pngOptions);

                    // Retrieve rendered image dimensions
                    int width = cadImage.Width;
                    int height = cadImage.Height;

                    // Output dimensions for quality assessment
                    Console.WriteLine($"Rendered image dimensions: {width}x{height}");
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}