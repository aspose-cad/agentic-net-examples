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
        string cadFilePath = "sample.dwg";

        if (!File.Exists(cadFilePath))
        {
            Console.WriteLine($"CAD file not found: {cadFilePath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(cadFilePath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to load CAD image.");
                    return;
                }

                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    ProcessEntity(entity, cadImage);
                }

                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Block: {block.Name}");
                    foreach (CadEntityBase blockEntity in block.Entities)
                    {
                        ProcessEntity(blockEntity, cadImage);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading CAD file: {ex.Message}");
        }
    }

    static void ProcessEntity(CadEntityBase entity, CadImage cadImage)
    {
        try
        {
            switch (entity)
            {
                case CadLine line:
                    Console.WriteLine($"Line: ({line.FirstPoint.X}, {line.FirstPoint.Y}, {line.FirstPoint.Z}) to ({line.SecondPoint.X}, {line.SecondPoint.Y}, {line.SecondPoint.Z}) on layer {line.LayerName}");
                    break;

                case CadLwPolyline polyline:
                    Console.WriteLine($"Lightweight Polyline on layer {polyline.LayerName} with {polyline.Coordinates.Count} points:");
                    foreach (var pt in polyline.Coordinates)
                    {
                        Console.WriteLine($"\tPoint: ({pt.X}, {pt.Y})");
                    }
                    break;

                case Cad3DFace face:
                    Console.WriteLine($"3D Face on layer {face.LayerName}");
                    Console.WriteLine($"\tCorners: ({face.FirstCorner.X}, {face.FirstCorner.Y}, {face.FirstCorner.Z}), ({face.SecondCorner.X}, {face.SecondCorner.Y}, {face.SecondCorner.Z}), ({face.ThirdCorner.X}, {face.ThirdCorner.Y}, {face.ThirdCorner.Z}), ({face.FourthCorner.X}, {face.FourthCorner.Y}, {face.FourthCorner.Z})");
                    break;

                case CadInsertObject insert:
                    Console.WriteLine($"Insert of block '{insert.Name}' on layer {insert.LayerName}");
                    Console.WriteLine($"\tScale: ({insert.ScaleX}, {insert.ScaleY}, {insert.ScaleZ})");
                    CadBlockEntity referencedBlock = FindBlockByName(cadImage, insert.Name);
                    if (referencedBlock != null)
                    {
                        foreach (CadEntityBase innerEntity in referencedBlock.Entities)
                        {
                            ProcessEntity(innerEntity, cadImage);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Warning: Block '{insert.Name}' not found.");
                    }
                    break;

                default:
                    Console.WriteLine($"Unhandled entity type: {entity.GetType().Name} on layer {entity.LayerName}");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing entity of type {entity.GetType().Name}: {ex.Message}");
        }
    }

    static CadBlockEntity FindBlockByName(CadImage cadImage, string blockName)
    {
        foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
        {
            if (string.Equals(block.Name, blockName, StringComparison.OrdinalIgnoreCase))
            {
                return block;
            }
        }
        return null;
    }
}