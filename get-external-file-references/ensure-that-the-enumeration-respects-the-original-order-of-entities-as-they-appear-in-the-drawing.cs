using System;
using System.IO;
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
            Console.Error.WriteLine($"File not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Enumerate top‑level entities in the order they appear
                Console.WriteLine("Top‑level entities:");
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    PrintEntityInfo(entity);
                }

                // Enumerate block definitions and their entities preserving order
                Console.WriteLine("\nBlock definitions:");
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Block: {block.Name}");
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        PrintEntityInfo(entity);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }

    static void PrintEntityInfo(CadEntityBase entity)
    {
        // Log the entity type
        string typeName = entity.GetType().Name;
        Console.Write($"Entity Type: {typeName}");

        // Provide details for known entity types
        if (entity is CadLine line)
        {
            Console.WriteLine($", Start: ({line.FirstPoint.X}, {line.FirstPoint.Y}, {line.FirstPoint.Z}), End: ({line.SecondPoint.X}, {line.SecondPoint.Y}, {line.SecondPoint.Z})");
        }
        else if (entity is CadLwPolyline polyline)
        {
            Console.Write(", Points: ");
            foreach (Cad2DPoint pt in polyline.Coordinates)
            {
                Console.Write($"({pt.X}, {pt.Y}) ");
            }
            Console.WriteLine();
        }
        else if (entity is Cad3DFace face)
        {
            Console.WriteLine($", Corners: ({face.FirstCorner.X}, {face.FirstCorner.Y}, {face.FirstCorner.Z}), ({face.SecondCorner.X}, {face.SecondCorner.Y}, {face.SecondCorner.Z}), ({face.ThirdCorner.X}, {face.ThirdCorner.Y}, {face.ThirdCorner.Z}), ({face.FourthCorner.X}, {face.FourthCorner.Y}, {face.FourthCorner.Z})");
        }
        else if (entity is CadInsertObject insert)
        {
            Console.WriteLine($", Inserted Block: {insert.Name}, Scale: ({insert.ScaleX}, {insert.ScaleY}, {insert.ScaleZ})");
        }
        else
        {
            Console.WriteLine();
        }
    }
}