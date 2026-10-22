using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadEntityDeletion
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file
                string sourcePath = "input.dwg";
                // Path to the output CAD file
                string outputPath = "output.dwg";

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Load the CAD file
                using (Image image = Image.Load(sourcePath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("Failed to load CAD image.");
                        return;
                    }

                    // Define the rectangle area (hatch‑filled) to test intersection.
                    double rectMinX = 100.0;
                    double rectMinY = 100.0;
                    double rectMaxX = 300.0;
                    double rectMaxY = 300.0;

                    // Helper local function to test if a 2D point lies inside the rectangle
                    bool IsPointInside(double x, double y) =>
                        x >= rectMinX && x <= rectMaxX && y >= rectMinY && y <= rectMaxY;

                    // Cast the entities collection to a mutable list if possible
                    if (cadImage.Entities is IList<CadEntityBase> mutableEntities)
                    {
                        for (int i = mutableEntities.Count - 1; i >= 0; i--)
                        {
                            CadEntityBase entity = mutableEntities[i];
                            bool shouldDelete = false;

                            // Check for line intersection
                            if (entity is CadLine line)
                            {
                                if (IsPointInside(line.FirstPoint.X, line.FirstPoint.Y) ||
                                    IsPointInside(line.SecondPoint.X, line.SecondPoint.Y))
                                {
                                    shouldDelete = true;
                                }
                            }
                            // Check for lightweight polyline intersection
                            else if (entity is CadLwPolyline polyline)
                            {
                                foreach (var pt in polyline.Coordinates)
                                {
                                    if (IsPointInside(pt.X, pt.Y))
                                    {
                                        shouldDelete = true;
                                        break;
                                    }
                                }
                            }
                            // Check for 3D face intersection (projected to XY plane)
                            else if (entity is Cad3DFace face)
                            {
                                Cad3DPoint[] corners = new[]
                                {
                                    face.FirstCorner,
                                    face.SecondCorner,
                                    face.ThirdCorner,
                                    face.FourthCorner
                                };
                                foreach (var pt in corners)
                                {
                                    if (IsPointInside(pt.X, pt.Y))
                                    {
                                        shouldDelete = true;
                                        break;
                                    }
                                }
                            }

                            if (shouldDelete)
                            {
                                mutableEntities.RemoveAt(i);
                                Console.WriteLine($"Deleted entity of type {entity.TypeName} at index {i}.");
                            }
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine("Unable to modify entities collection; it is not mutable.");
                        return;
                    }

                    // Save the modified CAD file
                    var dxfOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };
                    cadImage.Save(outputPath, dxfOptions);
                    Console.WriteLine("Modified CAD file saved.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}