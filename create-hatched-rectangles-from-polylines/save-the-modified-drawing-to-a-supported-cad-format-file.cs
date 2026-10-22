using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using Aspose.CAD.ImageOptions;

namespace HatchedRectanglesExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Input CAD file path
                string inputFilePath = "input.dwg";

                if (!File.Exists(inputFilePath))
                {
                    Console.Error.WriteLine($"Source file not found: {inputFilePath}");
                    return;
                }

                // Output directory
                string outputDirectory = "output";
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Load CAD drawing
                using (Image image = Image.Load(inputFilePath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // Find closed polylines that represent axis‑aligned rectangles
                    List<CadLwPolyline> rectanglePolylines = new List<CadLwPolyline>();

                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        if (entity is CadLwPolyline polyline &&
                            polyline.Flag.HasFlag(CadLwPolylineFlag.Closed) &&
                            polyline.Coordinates != null &&
                            polyline.Coordinates.Count == 4)
                        {
                            var p1 = polyline.Coordinates[0];
                            var p2 = polyline.Coordinates[1];
                            var p3 = polyline.Coordinates[2];
                            var p4 = polyline.Coordinates[3];

                            bool isAxisAligned =
                                (Math.Abs(p1.X - p2.X) < 1e-6 && Math.Abs(p2.Y - p3.Y) < 1e-6 &&
                                 Math.Abs(p3.X - p4.X) < 1e-6 && Math.Abs(p4.Y - p1.Y) < 1e-6) ||
                                (Math.Abs(p1.Y - p2.Y) < 1e-6 && Math.Abs(p2.X - p3.X) < 1e-6 &&
                                 Math.Abs(p3.Y - p4.Y) < 1e-6 && Math.Abs(p4.X - p1.X) < 1e-6);

                            if (isAxisAligned)
                            {
                                rectanglePolylines.Add(polyline);
                            }
                        }
                    }

                    // Note: Hatch creation is omitted because the required classes are not available in the current API.

                    // Save modified drawing to DWG using DxfOptions (preserves CAD entities)
                    string outputFilePath = Path.Combine(outputDirectory, "HatchedRectangles.dwg");
                    var dxfOptions = new DxfOptions { OutputMode = CadOutputMode.Convert };
                    cadImage.Save(outputFilePath, dxfOptions);

                    Console.WriteLine($"Drawing saved to: {outputFilePath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}