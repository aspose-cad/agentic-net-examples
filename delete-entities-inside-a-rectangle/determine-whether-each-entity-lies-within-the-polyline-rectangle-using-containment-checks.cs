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

class PolylineContainmentChecker
{
    static void Main()
    {
        // Input CAD file path
        string inputFilePath = @"C:\CAD\sample.dxf";

        // Output directory for the processed file
        string outputDir = @"C:\CAD\Processed";

        // Verify input file exists
        if (!File.Exists(inputFilePath))
        {
            Console.WriteLine($"Input file not found: {inputFilePath}");
            return;
        }

        // Ensure output directory exists
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD image using Aspose.CAD
            using (CadImage cadImage = (CadImage)Image.Load(inputFilePath))
            {
                // ------------------------------------------------------------
                // Define a rectangular polyline (closed) that will be used as
                // the containment area. The rectangle is defined in the XY plane.
                // ------------------------------------------------------------
                CadLwPolyline rectangle = new CadLwPolyline
                {
                    Flag = CadLwPolylineFlag.Closed
                };

                // Rectangle corners (counter‑clockwise)
                rectangle.Coordinates.Add(new Cad2DPoint(0, 0));
                rectangle.Coordinates.Add(new Cad2DPoint(200, 0));
                rectangle.Coordinates.Add(new Cad2DPoint(200, 100));
                rectangle.Coordinates.Add(new Cad2DPoint(0, 100));

                // Add the rectangle to the model space (entities collection)
                // Entities collection is read‑only, so we create a new collection
                // that includes the existing entities plus the rectangle.
                var updatedEntities = cadImage.Entities.Concat(new[] { rectangle }).ToArray();
                // Replace the internal collection via reflection (Aspose.CAD provides no direct setter,
                // but for the purpose of this sample we assume a helper method exists.
                // If not, the rectangle can be added to a block and the block referenced.
                // Here we use the BlockEntities collection to store the rectangle.
                CadBlockEntity modelSpace = cadImage.BlockEntities.ValuesTyped.FirstOrDefault(b => b.Name == "*Model_Space");
                if (modelSpace != null)
                {
                    modelSpace.AddEntity(rectangle);
                }

                // ------------------------------------------------------------
                // Containment check: iterate over all vertex entities and determine
                // whether their location lies inside the rectangle.
                // ------------------------------------------------------------
                // Simple axis‑aligned bounding‑box test because the rectangle is axis aligned.
                double minX = rectangle.Coordinates.Min(p => p.X);
                double maxX = rectangle.Coordinates.Max(p => p.X);
                double minY = rectangle.Coordinates.Min(p => p.Y);
                double maxY = rectangle.Coordinates.Max(p => p.Y);

                // List to collect points that are inside the rectangle
                List<CadVertexBase> insideVertices = new List<CadVertexBase>();

                // Check entities in model space
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    // We are interested in vertex entities (2D or 3D)
                    if (entity is CadVertexBase vertex)
                    {
                        Cad3DPoint pt = vertex.LocationPoint;

                        // Perform 2D containment test (ignore Z)
                        if (pt.X >= minX && pt.X <= maxX && pt.Y >= minY && pt.Y <= maxY)
                        {
                            insideVertices.Add(vertex);
                        }
                    }
                }

                Console.WriteLine($"Found {insideVertices.Count} vertex entities inside the rectangle.");

                // ------------------------------------------------------------
                // Visualise inside points by creating a 3D face (a thin square) at each location.
                // Demonstrates correct usage of Cad3DFace constructor.
                // ------------------------------------------------------------
                foreach (CadVertexBase vertex in insideVertices)
                {
                    Cad3DPoint p = vertex.LocationPoint;

                    // Define a small square (1 unit side) in the XY plane centered on the point
                    double halfSize = 0.5;
                    Cad3DPoint p1 = new Cad3DPoint(p.X - halfSize, p.Y - halfSize, p.Z);
                    Cad3DPoint p2 = new Cad3DPoint(p.X + halfSize, p.Y - halfSize, p.Z);
                    Cad3DPoint p3 = new Cad3DPoint(p.X + halfSize, p.Y + halfSize, p.Z);
                    Cad3DPoint p4 = new Cad3DPoint(p.X - halfSize, p.Y + halfSize, p.Z);

                    Cad3DFace face = new Cad3DFace(p1, p2, p3, p4);

                    // Add the face to model space
                    modelSpace?.AddEntity(face);
                }

                // ------------------------------------------------------------
                // Save the modified CAD file
                // ------------------------------------------------------------
                string outputFilePath = Path.Combine(outputDir, "sample_processed.dxf");
                cadImage.Save(outputFilePath, new DxfOptions { OutputMode = CadOutputMode.Convert });
                Console.WriteLine($"Processed CAD file saved to: {outputFilePath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}