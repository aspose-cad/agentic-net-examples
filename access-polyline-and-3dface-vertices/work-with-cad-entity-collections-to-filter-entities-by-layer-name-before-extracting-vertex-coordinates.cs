using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the CAD file
            string cadFilePath = "input.dwg";

            // Verify that the file exists before attempting to load it
            if (!File.Exists(cadFilePath))
            {
                Console.Error.WriteLine($"File not found: {cadFilePath}");
                return;
            }

            // Load the CAD file and cast to CadImage
            using (Image image = Image.Load(cadFilePath))
            {
                CadImage cadImage = (CadImage)image;

                // Layer name to filter entities
                string targetLayerName = "MyLayer";

                // Collection to hold extracted point information
                List<string> extractedPoints = new List<string>();

                // Iterate over top‑level entities in the drawing
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    // Filter entities by the specified layer name
                    if (!string.IsNullOrEmpty(entity.LayerName) &&
                        entity.LayerName.Equals(targetLayerName, StringComparison.OrdinalIgnoreCase))
                    {
                        // Process line entities
                        if (entity is CadLine line)
                        {
                            extractedPoints.Add($"Line Start: ({line.FirstPoint.X}, {line.FirstPoint.Y}, {line.FirstPoint.Z})");
                            extractedPoints.Add($"Line End: ({line.SecondPoint.X}, {line.SecondPoint.Y}, {line.SecondPoint.Z})");
                        }
                        // Process lightweight polyline entities
                        else if (entity is CadLwPolyline polyline)
                        {
                            foreach (Cad2DPoint point in polyline.Coordinates)
                            {
                                extractedPoints.Add($"Polyline Vertex: ({point.X}, {point.Y})");
                            }
                        }
                        // Process 3D face entities
                        else if (entity is Cad3DFace face)
                        {
                            extractedPoints.Add($"3DFace Corner1: ({face.FirstCorner.X}, {face.FirstCorner.Y}, {face.FirstCorner.Z})");
                            extractedPoints.Add($"3DFace Corner2: ({face.SecondCorner.X}, {face.SecondCorner.Y}, {face.SecondCorner.Z})");
                            extractedPoints.Add($"3DFace Corner3: ({face.ThirdCorner.X}, {face.ThirdCorner.Y}, {face.ThirdCorner.Z})");
                            extractedPoints.Add($"3DFace Corner4: ({face.FourthCorner.X}, {face.FourthCorner.Y}, {face.FourthCorner.Z})");
                        }
                        // Additional entity types can be handled here
                    }
                }

                // Output the extracted coordinates
                foreach (string info in extractedPoints)
                {
                    Console.WriteLine(info);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}