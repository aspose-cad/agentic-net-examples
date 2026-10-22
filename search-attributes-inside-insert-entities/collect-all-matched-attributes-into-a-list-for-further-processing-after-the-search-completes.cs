using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadBlockInspection
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the CAD file
                string cadFilePath = "input.dwg";

                // Ensure the file exists; create a minimal placeholder if missing
                if (!File.Exists(cadFilePath))
                {
                    // Create an empty placeholder file to avoid File IO errors
                    File.WriteAllBytes(cadFilePath, new byte[0]);
                }

                // List to collect matched attribute descriptions
                List<string> matchedAttributes = new List<string>();

                // Load the CAD image and cast to CadImage
                using (Image image = Image.Load(cadFilePath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("Failed to load CAD image.");
                        return;
                    }

                    // Inspect top‑level drawing entities
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        ProcessEntity(entity, matchedAttributes);
                    }

                    // Inspect block definitions
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            ProcessEntity(entity, matchedAttributes);
                        }
                    }
                }

                // Further processing of collected attributes
                Console.WriteLine("Collected Attributes:");
                foreach (string attribute in matchedAttributes)
                {
                    Console.WriteLine(attribute);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }

        // Helper method to process an entity and collect attributes
        private static void ProcessEntity(CadEntityBase entity, List<string> collection)
        {
            switch (entity)
            {
                case CadLine line:
                    // Calculate line length using 3D points
                    double dx = line.SecondPoint.X - line.FirstPoint.X;
                    double dy = line.SecondPoint.Y - line.FirstPoint.Y;
                    double dz = line.SecondPoint.Z - line.FirstPoint.Z;
                    double length = Math.Sqrt(dx * dx + dy * dy + dz * dz);
                    collection.Add($"Line Length: {length:F3}");
                    break;

                case CadLwPolyline polyline:
                    // Count number of vertices in lightweight polyline
                    int pointCount = polyline.Coordinates?.Count ?? 0;
                    collection.Add($"LwPolyline Vertex Count: {pointCount}");
                    break;

                case CadInsertObject insert:
                    // Record inserted block name and its scale factors
                    collection.Add($"Insert Block: {insert.Name}, ScaleX={insert.ScaleX}, ScaleY={insert.ScaleY}, ScaleZ={insert.ScaleZ}");
                    break;

                // Add more entity type handling as needed
                default:
                    // For other entities, store the type name
                    collection.Add($"Entity Type: {entity.GetType().Name}");
                    break;
            }
        }
    }
}