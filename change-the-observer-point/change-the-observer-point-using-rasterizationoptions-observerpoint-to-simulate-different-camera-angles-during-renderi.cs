using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats;
using Aspose.CAD.FileFormats.Cad;
using System.IO;

namespace CadObserverExample
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Path to the source CAD file
                    string inputPath = "input.dwg";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Path for the rendered image output
                    string outputPath = "output.png";

                    // Load the CAD drawing
                    using (Image cadImage = Image.Load(inputPath))
                    {
                        // Configure rasterization options
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                        // Set page size (in pixels)
                        rasterOptions.PageWidth = 1200;
                        rasterOptions.PageHeight = 800;
                        // Set unit type (optional)
                        rasterOptions.UnitType = UnitType.Millimeter;
                        // Set line scale (optional)
                        rasterOptions.LineScale = 1.0f;
                        // Define observer rotation angles (X, Y, Z) in degrees
                        // Example: rotate 30° around X axis, 15° around Y axis, no rotation around Z axis
                        rasterOptions.ObserverPoint = new ObserverPoint(30f, 15f, 0f);
                        // Optional: render only specific layouts (null renders all)
                        // rasterOptions.Layouts = null;

                        // Configure PNG export options and attach rasterization settings
                        PngOptions pngOptions = new PngOptions();
                        pngOptions.VectorRasterizationOptions = rasterOptions;

                        // Render and save the image
                        cadImage.Save(outputPath, pngOptions);
                    }

                    Console.WriteLine("Rendering completed. Image saved to: " + outputPath);
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
    }
}