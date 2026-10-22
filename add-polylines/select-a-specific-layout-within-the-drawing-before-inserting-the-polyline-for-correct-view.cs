using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadLayoutAndPolylineExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dwg";
            string outputPath = "output.png";
            string layoutName = "Layout1";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)image;

                    // Create a lightweight polyline (closed rectangle)
                    CadLwPolyline polyline = new CadLwPolyline();
                    List<Cad2DPoint> points = new List<Cad2DPoint>
                    {
                        new Cad2DPoint(0, 0),
                        new Cad2DPoint(200, 0),
                        new Cad2DPoint(200, 100),
                        new Cad2DPoint(0, 100),
                        new Cad2DPoint(0, 0) // close the polyline
                    };
                    polyline.Coordinates = points;

                    // Add the polyline to the specified layout block if it exists
                    if (cadImage.BlockEntities.TryGetValue(layoutName, out CadBlockEntity layoutBlock))
                    {
                        layoutBlock.Entities.Add(polyline);
                    }
                    else if (cadImage.BlockEntities.TryGetValue("*Model_Space", out CadBlockEntity modelBlock))
                    {
                        modelBlock.Entities.Add(polyline);
                        Console.WriteLine($"Layout '{layoutName}' not found. Polyline added to Model Space.");
                    }
                    else
                    {
                        Console.WriteLine("No suitable block found to add the polyline.");
                    }

                    // Define rasterization options for the desired layout
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        Layouts = new[] { layoutName },
                        PageWidth = 1600,
                        PageHeight = 1200
                    };

                    // Prepare PNG export options
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Ensure output directory exists
                    try
                    {
                        string outputDir = Path.GetDirectoryName(outputPath);
                        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                        {
                            Directory.CreateDirectory(outputDir);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error preparing output directory: {ex.Message}");
                        return;
                    }

                    // Save the modified drawing as a raster image
                    try
                    {
                        cadImage.Save(outputPath, pngOptions);
                        Console.WriteLine($"Image saved to {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error saving image: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing CAD file: {ex.Message}");
            }
        }
    }
}
