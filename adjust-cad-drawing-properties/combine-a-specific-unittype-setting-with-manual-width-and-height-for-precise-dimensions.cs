using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace AdjustCadDrawingProperties
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file
                string sourcePath = "sample.dwg";
                // Path for the output PNG file
                string outputPath = "output.png";

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Load the CAD drawing
                using (Image image = Image.Load(sourcePath))
                {
                    // Cast to CadImage to access CAD-specific members if needed
                    CadImage cadImage = (CadImage)image;

                    // Configure rasterization options with a specific UnitType and manual dimensions
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Use Millimeter as the unit type
                        UnitType = UnitType.Millimeter,
                        // Set the desired output size in pixels
                        PageWidth = 1200,
                        PageHeight = 800,
                        // Optional: set background color (default is white)
                        BackgroundColor = Aspose.CAD.Color.White
                    };

                    // Set up PNG export options and attach the rasterization settings
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the drawing as PNG with the specified options
                    cadImage.Save(outputPath, pngOptions);
                }

                Console.WriteLine("CAD drawing has been saved to: " + outputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}
