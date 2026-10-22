using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadViewportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the source IFC file
            string dataDir = @"C:\CadSamples\";
            string sourceFile = Path.Combine(dataDir, "sample.ifc");

            // Verify source file exists
            if (!File.Exists(sourceFile))
            {
                Console.WriteLine($"Source file not found: {sourceFile}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }

            try
            {
                // Load the IFC drawing
                using (Image cadImage = Image.Load(sourceFile))
                {
                    // Configure rasterization options for custom viewport dimensions
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        UnitType = UnitType.Centimenter, // SDK spelling for centimeters
                        LineScale = 1.0f,                // Default line scale
                        PageWidth = 800f,                // Custom width (in the chosen unit)
                        PageHeight = 600f,               // Custom height (in the chosen unit)
                        Margins = new Margins
                        {
                            Left = 10,
                            Right = 10,
                            Top = 10,
                            Bottom = 10
                        }
                    };

                    // Prepare PNG export options and assign rasterization settings
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Path for the exported PNG file
                    string outputFile = Path.Combine(dataDir, "sample_output.png");

                    // Save the drawing as PNG with the custom viewport
                    cadImage.Save(outputFile, pngOptions);
                    Console.WriteLine($"PNG exported successfully to: {outputFile}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during processing: {ex.Message}");
            }
        }
    }
}