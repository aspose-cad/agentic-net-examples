using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using System.IO;

namespace CadEntityDeletion
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Input and output file paths
                    string inputPath = "input.dwg";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        string outputPath = "output.dxf";

                    // Define rectangular area (example coordinates)
                    double minX = 0.0;
                    double minY = 0.0;
                    double maxX = 100.0;
                    double maxY = 100.0;

                    // Create a closed 2‑D polyline representing the rectangle
                    CadLwPolyline rectangle = new CadLwPolyline();
                    rectangle.Flag = CadLwPolylineFlag.Closed;
                    rectangle.Coordinates = new List<Cad2DPoint>
                    {
                        new Cad2DPoint(minX, minY),
                        new Cad2DPoint(maxX, minY),
                        new Cad2DPoint(maxX, maxY),
                        new Cad2DPoint(minX, maxY)
                    };
                    rectangle.PointCount = rectangle.Coordinates.Count;

                    // Load the CAD drawing
                    using (Image image = Image.Load(inputPath))
                    {
                        // Cast to CadImage to access CAD object model
                        CadImage cadImage = (CadImage)image;

                        // Filter out entities that lie completely inside the rectangle
                        IEnumerable<CadEntityBase> filteredEntities = cadImage.Entities.Where(entity =>
                        {
                            // Only process lightweight polylines for this example
                            CadLwPolyline polyline = entity as CadLwPolyline;
                            if (polyline == null)
                                return true; // keep non‑polyline entities

                            // If any vertex is outside the rectangle, keep the entity
                            foreach (Cad2DPoint pt in polyline.Coordinates)
                            {
                                if (pt.X < minX || pt.X > maxX || pt.Y < minY || pt.Y > maxY)
                                    return true; // keep
                            }

                            // All vertices are inside – exclude this entity
                            return false;
                        }).ToArray();

                        // Replace the entity collection with the filtered one
                        cadImage.Entities = filteredEntities;

                        // Save the modified drawing as DXF
                        DxfOptions dxfOptions = new DxfOptions
                        {
                            OutputMode = CadOutputMode.Convert
                        };
                        cadImage.Save(outputPath, dxfOptions);
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