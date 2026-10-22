using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;

namespace AsposeCadPolylineInspection
{
    class Program
    {
        static void Main(string[] args)
        {
            const string inputFilePath = "input.dxf";

            try
            {
                if (!File.Exists(inputFilePath))
                {
                    CreatePlaceholderDxf(inputFilePath);
                }

                using (Image image = Image.Load(inputFilePath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    IList<Cad2DPoint> expectedVertices = new List<Cad2DPoint>
                    {
                        new Cad2DPoint(0, 0),
                        new Cad2DPoint(100, 0),
                        new Cad2DPoint(100, 100),
                        new Cad2DPoint(0, 100),
                        new Cad2DPoint(0, 0) // Closing the polyline
                    };

                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        if (entity is CadPolyline || entity is CadLwPolyline)
                        {
                            IList<Cad2DPoint> actualVertices = GetPolylineVertices(entity);
                            Console.WriteLine($"Found Polyline with {actualVertices.Count} vertices.");
                            bool match = CompareVertices(actualVertices, expectedVertices);
                            Console.WriteLine($"Geometry matches expected: {match}");
                        }
                    }

                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped) {
                        foreach (CadEntityBase blockEntity in block.Entities)
                        {
                            if (blockEntity is CadPolyline || blockEntity is CadLwPolyline)
                            {
                                IList<Cad2DPoint> actualVertices = GetPolylineVertices(blockEntity);
                                Console.WriteLine($"Found Polyline in block \"{block.Name}\" with {actualVertices.Count} vertices.");
                                bool match = CompareVertices(actualVertices, expectedVertices);
                                Console.WriteLine($"Geometry matches expected: {match}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during processing: {ex.Message}");
            }
        }

        private static IList<Cad2DPoint> GetPolylineVertices(CadEntityBase entity)
        {
            var vertices = new List<Cad2DPoint>();

            if (entity is CadPolyline polyline)
            {
                foreach (CadEntityBase child in polyline.ChildObjects)
                {
                    if (child is CadVertexBase vertex && vertex.LocationPoint != null)
                    {
                        var pt = vertex.LocationPoint; // Cad3DPoint
                        vertices.Add(new Cad2DPoint(pt.X, pt.Y));
                    }
                }
            }
            else if (entity is CadLwPolyline lwPolyline)
            {
                // Coordinates already provides a list of Cad2DPoint
                vertices.AddRange(lwPolyline.Coordinates);
            }

            return vertices;
        }

        private static void CreatePlaceholderDxf(string path)
        {
            string[] dxfLines = new string[]
            {
                "0","SECTION","2","ENTITIES",
                "0","POLYLINE","8","0","66","1","70","1",
                "0","VERTEX","8","0","10","0","20","0","30","0",
                "0","VERTEX","8","0","10","100","20","0","30","0",
                "0","VERTEX","8","0","10","100","20","100","30","0",
                "0","VERTEX","8","0","10","0","20","100","30","0",
                "0","VERTEX","8","0","10","0","20","0","30","0",
                "0","SEQEND","0","ENDSEC","0","EOF"
            };
            File.WriteAllLines(path, dxfLines);
        }

        private static bool CompareVertices(IList<Cad2DPoint> actual, IList<Cad2DPoint> expected, double tolerance = 1e-6)
        {
            if (actual == null || expected == null) return false;
            if (actual.Count != expected.Count) return false;

            for (int i = 0; i < actual.Count; i++)
            {
                var a = actual[i];
                var e = expected[i];
                if (Math.Abs(a.X - e.X) > tolerance || Math.Abs(a.Y - e.Y) > tolerance)
                    return false;
            }
            return true;
        }
    }
}