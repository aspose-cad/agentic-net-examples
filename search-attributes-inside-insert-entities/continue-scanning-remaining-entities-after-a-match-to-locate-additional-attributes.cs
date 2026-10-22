using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadBlockEntityInspection
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the CAD file (DWG or DXF)
                string cadFilePath = "sample.dwg";

                // Ensure the file exists; create a minimal placeholder if it does not
                if (!File.Exists(cadFilePath))
                {
                    Console.WriteLine($"File '{cadFilePath}' not found. Creating placeholder file.");
                    File.WriteAllBytes(cadFilePath, new byte[0]);
                }

                // Load the CAD file and cast to CadImage
                using (Image image = Image.Load(cadFilePath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("Failed to load CAD image.");
                        return;
                    }

                    // Iterate through all block definitions in the drawing
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        Console.WriteLine($"Inspecting Block: {block.Name}");

                        // Flag to indicate whether a matching entity was found
                        bool matchFound = false;

                        // Scan each entity inside the block
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            // Example match: find the first line entity
                            if (!matchFound && entity is CadLine line)
                            {
                                Console.WriteLine($"  Matched Line in Block '{block.Name}':");
                                Console.WriteLine($"    Layer: {line.LayerName}");
                                Console.WriteLine($"    Start Point: ({line.FirstPoint.X}, {line.FirstPoint.Y}, {line.FirstPoint.Z})");
                                Console.WriteLine($"    End Point:   ({line.SecondPoint.X}, {line.SecondPoint.Y}, {line.SecondPoint.Z})");
                                matchFound = true;
                                // Continue scanning remaining entities after the match
                                continue;
                            }

                            // After a match, locate additional attributes (e.g., inserts or polylines)
                            if (entity is CadInsertObject insert)
                            {
                                Console.WriteLine($"  Insert Object:");
                                Console.WriteLine($"    Name: {insert.Name}");
                                Console.WriteLine($"    Original Block Name: {insert.OriginalBlockName}");
                                Console.WriteLine($"    ScaleX: {insert.ScaleX}, ScaleY: {insert.ScaleY}, ScaleZ: {insert.ScaleZ}");
                            }
                            else if (entity is CadLwPolyline polyline)
                            {
                                Console.WriteLine($"  Lightweight Polyline with {polyline.Coordinates.Count} points:");
                                int pointIndex = 0;
                                foreach (Cad2DPoint pt in polyline.Coordinates)
                                {
                                    Console.WriteLine($"    Point {pointIndex}: ({pt.X}, {pt.Y})");
                                    pointIndex++;
                                }
                            }
                            else
                            {
                                // Log other entity types
                                Console.WriteLine($"  Entity Type: {entity.GetType().Name}, Layer: {entity.LayerName}");
                            }
                        }
                    }
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