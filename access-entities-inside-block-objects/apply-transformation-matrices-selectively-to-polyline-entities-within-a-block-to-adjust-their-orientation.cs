using System;
using System.Numerics;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;
using System.IO;

namespace CadTransformationExample
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Input and output CAD file paths
                    string inputPath = "input.dwg";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        string outputPath = "output.dwg";

                    // Load the CAD file and cast to CadImage
                    using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                    {
                        // Locate the block named "MyBlock"
                        CadBlockEntity targetBlock = null;
                        foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                        {
                            if (block.Name == "MyBlock")
                            {
                                targetBlock = block;
                                break;
                            }
                        }

                        if (targetBlock != null)
                        {
                            // Define a 90-degree rotation matrix (counter‑clockwise) around the origin
                            float angleRadians = (float)(Math.PI / 2);
                            Matrix3x2 rotationMatrix = Matrix3x2.CreateRotation(angleRadians);

                            // Iterate over entities inside the block
                            foreach (CadEntityBase entity in targetBlock.Entities)
                            {
                                // Apply transformation only to lightweight polylines
                                if (entity is CadLwPolyline polyline)
                                {
                                    // Transform each vertex of the polyline
                                    for (int i = 0; i < polyline.Coordinates.Count; i++)
                                    {
                                        Cad2DPoint originalPoint = polyline.Coordinates[i];

                                        // Convert to Vector2 for transformation
                                        Vector2 pointVector = new Vector2((float)originalPoint.X, (float)originalPoint.Y);
                                        Vector2 transformedVector = Vector2.Transform(pointVector, rotationMatrix);

                                        // Update the point with transformed coordinates
                                        Cad2DPoint transformedPoint = new Cad2DPoint
                                        {
                                            X = transformedVector.X,
                                            Y = transformedVector.Y
                                        };

                                        polyline.Coordinates[i] = transformedPoint;
                                    }
                                }
                            }
                        }

                        // Save the modified CAD drawing
                        cadImage.Save(outputPath);
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