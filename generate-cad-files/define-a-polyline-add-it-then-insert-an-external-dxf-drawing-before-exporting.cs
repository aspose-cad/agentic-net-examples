using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;

namespace AsposeCadPolylineExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string mainInputPath = "input.dxf";
            string externalInputPath = "external.dxf";
            string outputPath = "output.dxf";

            try
            {
                if (!File.Exists(mainInputPath))
                {
                    Console.Error.WriteLine($"Main input file not found: {mainInputPath}");
                    return;
                }

                if (!File.Exists(externalInputPath))
                {
                    Console.Error.WriteLine($"External DXF file not found: {externalInputPath}");
                    return;
                }

                // Load the main CAD drawing
                using (CadImage mainImage = (CadImage)Image.Load(mainInputPath))
                {
                    // Define a closed 2D polyline
                    CadLwPolyline polyline = new CadLwPolyline();
                    polyline.Coordinates = new List<Cad2DPoint>
                    {
                        new Cad2DPoint(0, 0),
                        new Cad2DPoint(100, 0),
                        new Cad2DPoint(100, 100),
                        new Cad2DPoint(0, 100)
                    };
                    polyline.Flag = CadLwPolylineFlag.Closed;
                    polyline.PointCount = polyline.Coordinates.Count;

                    // Append the polyline to the existing entities
                    CadEntityBase[] updatedEntities = mainImage.Entities
                        .Concat(new CadEntityBase[] { polyline })
                        .ToArray();
                    mainImage.Entities = updatedEntities;

                    // Load the external DXF drawing
                    using (CadImage externalImage = (CadImage)Image.Load(externalInputPath))
                    {
                        // Merge external entities into the main drawing
                        CadEntityBase[] mergedEntities = mainImage.Entities
                            .Concat(externalImage.Entities)
                            .ToArray();
                        mainImage.Entities = mergedEntities;
                    }

                    // Save the resulting drawing as DXF
                    DxfOptions saveOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };
                    mainImage.Save(outputPath, saveOptions);
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