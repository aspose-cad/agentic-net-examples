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
        const string cadFilePath = "sample.dwg"; // Replace with actual path or placeholder

        if (!File.Exists(cadFilePath))
        {
            Console.Error.WriteLine($"CAD file not found: {cadFilePath}");
            return;
        }

        try
        {
            // Load the CAD drawing from file. The Image object implements IDisposable.
            using (Image image = Image.Load(cadFilePath))
            {
                // Cast the generic Image to CadImage to access CAD‑specific members.
                CadImage cadImage = (CadImage)image;

                // Process top‑level drawing entities.
                Console.WriteLine("Top‑level entities:");
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    ProcessEntity(entity, cadImage);
                }

                // Process block definitions stored in the drawing.
                Console.WriteLine("\nBlock definitions:");
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Block: {block.Name}");
                    foreach (CadEntityBase blockEntity in block.Entities)
                    {
                        ProcessEntity(blockEntity, cadImage);
                    }
                }
            } // Image (and underlying CadImage) is disposed here.
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }

    // Recursively processes a CAD entity, handling known types and block inserts.
    static void ProcessEntity(CadEntityBase entity, CadImage cadImage)
    {
        switch (entity)
        {
            case CadLine line:
                Console.WriteLine($"Line on layer {line.LayerName}: ({line.FirstPoint.X}, {line.FirstPoint.Y}, {line.FirstPoint.Z}) -> ({line.SecondPoint.X}, {line.SecondPoint.Y}, {line.SecondPoint.Z})");
                break;

            case CadLwPolyline polyline:
                Console.WriteLine($"LwPolyline on layer {polyline.LayerName} with {polyline.Coordinates.Count} points");
                foreach (Cad2DPoint pt in polyline.Coordinates)
                {
                    Console.WriteLine($"  Point: ({pt.X}, {pt.Y})");
                }
                break;

            case Cad3DFace face:
                Console.WriteLine($"3DFace on layer {face.LayerName}");
                Console.WriteLine($"  Corners: ({face.FirstCorner.X}, {face.FirstCorner.Y}, {face.FirstCorner.Z}), ({face.SecondCorner.X}, {face.SecondCorner.Y}, {face.SecondCorner.Z}), ({face.ThirdCorner.X}, {face.ThirdCorner.Y}, {face.ThirdCorner.Z}), ({face.FourthCorner.X}, {face.FourthCorner.Y}, {face.FourthCorner.Z})");
                break;

            case CadInsertObject insert:
                Console.WriteLine($"Insert of block '{insert.Name}' on layer {insert.LayerName} with scale ({insert.ScaleX}, {insert.ScaleY}, {insert.ScaleZ})");
                // Locate the block definition referenced by the insert.
                CadBlockEntity referencedBlock = FindBlockByName(cadImage, insert.Name);
                if (referencedBlock != null)
                {
                    // Recursively process entities inside the inserted block.
                    foreach (CadEntityBase innerEntity in referencedBlock.Entities)
                    {
                        ProcessEntity(innerEntity, cadImage);
                    }
                }
                else
                {
                    Console.WriteLine($"  Block definition '{insert.Name}' not found.");
                }
                break;

            default:
                // Fallback for any other entity types.
                Console.WriteLine($"Entity type: {entity.GetType().Name} on layer {entity.LayerName}");
                break;
        }
    }

    // Helper method to locate a block definition by its name.
    static CadBlockEntity FindBlockByName(CadImage cadImage, string blockName)
    {
        foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
        {
            if (block.Name.Equals(blockName, StringComparison.OrdinalIgnoreCase))
            {
                return block;
            }
        }
        return null;
    }
}