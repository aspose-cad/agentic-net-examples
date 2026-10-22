using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats;
using Aspose.CAD.FileFormats.Cad;

namespace CadObserverExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file
                string sourcePath = "model.dxf";

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = "output";
                Directory.CreateDirectory(outputDir);

                // Load the CAD image
                using (CadImage cadImage = (CadImage)Image.Load(sourcePath))
                {
                    // Define multiple observer points (rotation angles in degrees)
                    ObserverPoint[] observerPoints = new ObserverPoint[]
                    {
                        new ObserverPoint(0f, 0f, 0f),          // Front view
                        new ObserverPoint(30f, 0f, 0f),         // Tilted up 30°
                        new ObserverPoint(0f, 45f, 0f),         // Rotate around Y axis 45°
                        new ObserverPoint(0f, 0f, 60f)          // Rotate around Z axis 60°
                    };

                    // Iterate through each observer point and render an image
                    for (int i = 0; i < observerPoints.Length; i++)
                    {
                        // Configure rasterization options for the current perspective
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                        {
                            ObserverPoint = observerPoints[i],
                            UnitType = UnitType.Millimeter,
                            PageWidth = 800,
                            PageHeight = 600,
                            LineScale = 0.5f,
                            AutomaticLayoutsScaling = true
                        };

                        // Set PNG output options
                        PngOptions pngOptions = new PngOptions
                        {
                            VectorRasterizationOptions = rasterOptions
                        };

                        // Build output file name
                        string outputPath = Path.Combine(outputDir, $"model_view_{i + 1}.png");

                        // Save the rendered image
                        cadImage.Save(outputPath, pngOptions);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}