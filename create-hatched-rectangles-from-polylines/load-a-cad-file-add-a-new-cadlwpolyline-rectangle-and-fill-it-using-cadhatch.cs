using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using Aspose.CAD.ImageOptions;

namespace CadRectangleWithHatch
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dwg";
            string outputPath = "output.dxf";

            try
            {
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Create a closed 2D polyline representing a rectangle
                    CadLwPolyline rectangle = new CadLwPolyline
                    {
                        Flag = CadLwPolylineFlag.Closed,
                        Coordinates = new List<Cad2DPoint>
                        {
                            new Cad2DPoint(0, 0),
                            new Cad2DPoint(100, 0),
                            new Cad2DPoint(100, 50),
                            new Cad2DPoint(0, 50)
                        }
                    };
                    rectangle.PointCount = rectangle.Coordinates.Count;

                    // Create a solid-filled face to simulate a hatch
                    Cad3DFace hatchFace = new Cad3DFace(
                        new Cad3DPoint(0, 0, 0),
                        new Cad3DPoint(100, 0, 0),
                        new Cad3DPoint(100, 50, 0),
                        new Cad3DPoint(0, 50, 0));

                    // Append new entities to the existing collection
                    CadEntityBase[] newEntities = cadImage.Entities
                        .Concat(new CadEntityBase[] { rectangle, hatchFace })
                        .ToArray();

                    cadImage.Entities = newEntities;

                    // Save the modified CAD drawing
                    DxfOptions saveOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };
                    cadImage.Save(outputPath, saveOptions);
                }

                Console.WriteLine($"File saved successfully to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}