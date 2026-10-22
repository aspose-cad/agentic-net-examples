using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;
using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source CAD file
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD‑specific members
                CadImage cadImage = (CadImage)image;

                // Adjust line weight for all entities except LWPOLYLINE
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (!(entity is CadLwPolyline))
                    {
                        // Example line weight value (in hundredths of mm)
                        entity.LineWeight = 25;
                    }
                }

                // Also adjust entities inside block definitions
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    foreach (CadEntityBase blockEntity in block.Entities)
                    {
                        if (!(blockEntity is CadLwPolyline))
                        {
                            blockEntity.LineWeight = 25;
                        }
                    }
                }

                // Enumerate layers
                Console.WriteLine("Layers:");
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    Console.WriteLine($"  Name: {layer.Name}, ColorId: {layer.ColorId}, Flags: {layer.Flags}");
                }

                // Enumerate top‑level entities
                Console.WriteLine("\nTop‑level entities:");
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    PrintEntityInfo(entity, "  ");
                }

                // Enumerate block definitions
                Console.WriteLine("\nBlock definitions:");
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Block name: {block.Name}, Entity count: {block.Entities.Count}");
                    foreach (CadEntityBase blockEntity in block.Entities)
                    {
                        PrintEntityInfo(blockEntity, "    ");
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

    // Helper method to display information about supported entity types
    static void PrintEntityInfo(CadEntityBase entity, string indent)
    {
        if (entity is CadLine line)
        {
            Console.WriteLine($"{indent}Line: Start({line.FirstPoint.X}, {line.FirstPoint.Y}, {line.FirstPoint.Z}) " +
                              $"End({line.SecondPoint.X}, {line.SecondPoint.Y}, {line.SecondPoint.Z}) " +
                              $"Layer: {line.LayerName}");
        }
        else if (entity is CadLwPolyline polyline)
        {
            Console.WriteLine($"{indent}LwPolyline: Points={polyline.Coordinates.Count} Layer: {polyline.LayerName}");
        }
        else if (entity is Cad3DFace face)
        {
            Console.WriteLine($"{indent}3DFace: Corner1({face.FirstCorner.X}, {face.FirstCorner.Y}, {face.FirstCorner.Z}) " +
                              $"Corner2({face.SecondCorner.X}, {face.SecondCorner.Y}, {face.SecondCorner.Z}) " +
                              $"Corner3({face.ThirdCorner.X}, {face.ThirdCorner.Y}, {face.ThirdCorner.Z}) " +
                              $"Corner4({face.FourthCorner.X}, {face.FourthCorner.Y}, {face.FourthCorner.Z}) " +
                              $"Layer: {face.LayerName}");
        }
        else if (entity is CadInsertObject insert)
        {
            Console.WriteLine($"{indent}Insert: BlockName={insert.Name} " +
                              $"InsertionPoint=({insert.InsertionPoint.X}, {insert.InsertionPoint.Y}, {insert.InsertionPoint.Z}) " +
                              $"Scale=({insert.ScaleX}, {insert.ScaleY}, {insert.ScaleZ}) " +
                              $"Rotation={insert.RotationAngle} Layer={insert.LayerName}");
        }
        else
        {
            Console.WriteLine($"{indent}Unsupported entity type: {entity.GetType().Name}");
        }
    }
}