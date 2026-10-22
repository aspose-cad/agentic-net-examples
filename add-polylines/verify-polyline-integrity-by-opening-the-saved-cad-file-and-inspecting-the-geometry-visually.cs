using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"File not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                var cadImage = (CadImage)image;

                // Inspect top‑level drawing entities
                foreach (var entity in cadImage.Entities)
                {
                    if (entity is CadLwPolyline polyline)
                    {
                        Console.WriteLine("Top‑level Polyline found:");
                        int vertexIndex = 0;
                        foreach (var point in polyline.Coordinates)
                        {
                            Console.WriteLine($"  Vertex {vertexIndex}: X={point.X}, Y={point.Y}");
                            vertexIndex++;
                        }
                    }
                    else if (entity is CadInsertObject insert)
                    {
                        // Recursively inspect inserted block references
                        InspectInsert(insert, cadImage);
                    }
                }

                // Inspect block definitions stored in the drawing
                foreach (var block in cadImage.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Block definition: {block.Name}");
                    foreach (var blockEntity in block.Entities)
                    {
                        if (blockEntity is CadLwPolyline polyline)
                        {
                            Console.WriteLine($"  Polyline in block {block.Name}:");
                            int vertexIndex = 0;
                            foreach (var point in polyline.Coordinates)
                            {
                                Console.WriteLine($"    Vertex {vertexIndex}: X={point.X}, Y={point.Y}");
                                vertexIndex++;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }

    static void InspectInsert(CadInsertObject insert, CadImage cadImage)
    {
        // Determine the referenced block name
        string blockName = insert.Name ?? insert.OriginalBlockName;

        // Locate the block definition
        var block = cadImage.BlockEntities.ValuesTyped.FirstOrDefault(b => b.Name == blockName);
        if (block == null)
        {
            Console.WriteLine($"Block \"{blockName}\" not found.");
            return;
        }

        Console.WriteLine($"Inspecting inserted block: {blockName}");

        foreach (var entity in block.Entities)
        {
            if (entity is CadLwPolyline polyline)
            {
                Console.WriteLine($"  Polyline in inserted block {blockName}:");
                int vertexIndex = 0;
                foreach (var point in polyline.Coordinates)
                {
                    // Apply insert scaling factors
                    double scaleX = insert.ScaleX != 0 ? insert.ScaleX : 1;
                    double scaleY = insert.ScaleY != 0 ? insert.ScaleY : 1;
                    double scaledX = point.X * scaleX;
                    double scaledY = point.Y * scaleY;

                    Console.WriteLine($"    Vertex {vertexIndex}: X={scaledX}, Y={scaledY}");
                    vertexIndex++;
                }
            }
            else if (entity is CadInsertObject nestedInsert)
            {
                // Recursively handle nested block inserts
                InspectInsert(nestedInsert, cadImage);
            }
        }
    }
}
