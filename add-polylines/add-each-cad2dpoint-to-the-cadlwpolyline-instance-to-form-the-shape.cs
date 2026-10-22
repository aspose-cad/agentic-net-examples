using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

namespace AddPolylinesExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input and output file paths (replace with actual paths as needed)
            string inputPath = "input.dwg";
            string outputPath = "output.dwg";

            try
            {
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Define the points of the polyline
                    List<Cad2DPoint> points = new List<Cad2DPoint>
                    {
                        new Cad2DPoint(0, 0),
                        new Cad2DPoint(100, 0),
                        new Cad2DPoint(100, 100),
                        new Cad2DPoint(0, 100),
                        new Cad2DPoint(0, 0) // Closing the shape
                    };

                    // Create a lightweight polyline and add the points
                    CadLwPolyline polyline = new CadLwPolyline();
                    polyline.Coordinates.AddRange(points);
                    polyline.Flag = CadLwPolylineFlag.Closed;
                    polyline.LayerName = "MyLayer";

                    // Add the polyline to the Model Space block entities
                    if (cadImage.BlockEntities.TryGetValue("*Model_Space", out CadBlockEntity modelSpace))
                    {
                        modelSpace.AddEntity(polyline);
                    }
                    else
                    {
                        Console.Error.WriteLine("Model space block not found in the CAD file.");
                        return;
                    }

                    // Save the modified drawing
                    cadImage.Save(outputPath);
                }

                Console.WriteLine($"Polyline added and file saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
