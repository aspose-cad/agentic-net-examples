using System;
using System.Collections;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string filePath = "sample.dwg";

        if (!File.Exists(filePath))
        {
            Console.Error.WriteLine($"File not found: {filePath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(filePath))
            {
                CadImage cadImage = (CadImage)image;

                Console.WriteLine("Top‑level entities:");
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    ProcessEntity(entity, cadImage);
                }

                Console.WriteLine("\nBlock definitions:");
                foreach (DictionaryEntry blockEntry in cadImage.BlockEntities)
                {
                    string blockName = (string)blockEntry.Key;
                    var blockEntity = blockEntry.Value as CadBlockEntity;
                    Console.WriteLine($"Block: {blockName}");
                    if (blockEntity != null)
                    {
                        foreach (CadEntityBase entity in blockEntity.Entities)
                        {
                            ProcessEntity(entity, cadImage, "  ");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }

    static void ProcessEntity(CadEntityBase entity, CadImage cadImage, string indent = "")
    {
        Console.WriteLine($"{indent}Entity Type: {entity.GetType().Name}");

        if (entity is CadLine line)
        {
            Console.WriteLine($"{indent}  Line from ({line.FirstPoint.X}, {line.FirstPoint.Y}, {line.FirstPoint.Z}) " +
                              $"to ({line.SecondPoint.X}, {line.SecondPoint.Y}, {line.SecondPoint.Z})");
        }
        else if (entity is CadLwPolyline polyline)
        {
            Console.WriteLine($"{indent}  Polyline points:");
            foreach (var pt in polyline.Coordinates)
            {
                Console.WriteLine($"{indent}    ({pt.X}, {pt.Y})");
            }
        }
        else if (entity is Cad3DFace face)
        {
            Console.WriteLine($"{indent}  3DFace corners:");
            Console.WriteLine($"{indent}    1: ({face.FirstCorner.X}, {face.FirstCorner.Y}, {face.FirstCorner.Z})");
            Console.WriteLine($"{indent}    2: ({face.SecondCorner.X}, {face.SecondCorner.Y}, {face.SecondCorner.Z})");
            Console.WriteLine($"{indent}    3: ({face.ThirdCorner.X}, {face.ThirdCorner.Y}, {face.ThirdCorner.Z})");
            Console.WriteLine($"{indent}    4: ({face.FourthCorner.X}, {face.FourthCorner.Y}, {face.FourthCorner.Z})");
        }
        else if (entity is CadInsertObject insert)
        {
            string referencedBlock = insert.Name;
            Console.WriteLine($"{indent}  Insert block reference: {referencedBlock}");

            if (cadImage.BlockEntities.Contains(referencedBlock))
            {
                var innerBlock = cadImage.BlockEntities[referencedBlock] as CadBlockEntity;
                if (innerBlock != null)
                {
                    foreach (CadEntityBase innerEntity in innerBlock.Entities)
                    {
                        ProcessEntity(innerEntity, cadImage, indent + "  ");
                    }
                }
            }
        }
    }
}