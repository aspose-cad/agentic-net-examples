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

namespace AsposeCadPolylineExample
{
    class Program
    {
        static void Main()
        {
            string inputPath = "input.dxf";
            string outputPath = "output.dxf";

            try
            {
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load existing DXF file
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Create a closed 2D lightweight polyline
                    CadLwPolyline polyline = new CadLwPolyline
                    {
                        Flag = CadLwPolylineFlag.Closed,
                        Coordinates = new List<Cad2DPoint>
                        {
                            new Cad2DPoint(0, 0),
                            new Cad2DPoint(100, 0),
                            new Cad2DPoint(100, 100),
                            new Cad2DPoint(0, 100)
                        }
                    };
                    polyline.PointCount = polyline.Coordinates.Count;

                    // Append the new polyline to the drawing entities
                    CadEntityBase[] updatedEntities = cadImage.Entities
                        .Concat(new CadEntityBase[] { polyline })
                        .ToArray();

                    // Replace the entities collection with the updated one
                    cadImage.Entities = updatedEntities;

                    // Save the modified drawing as DXF
                    DxfOptions saveOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };
                    cadImage.Save(outputPath);
                }

                Console.WriteLine($"Drawing saved successfully to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}