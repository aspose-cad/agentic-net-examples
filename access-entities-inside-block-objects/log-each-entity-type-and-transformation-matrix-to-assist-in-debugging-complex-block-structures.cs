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
        try
        {
                const string filePath = "sample.dwg";

                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"File not found: {filePath}");
                    return;
                }

                using (Image image = Image.Load(filePath))
                {
                    CadImage cadImage = (CadImage)image;

                    // Process top‑level entities
                    foreach (CadEntityBase topEntity in cadImage.Entities)
                    {
                        ProcessEntity(topEntity, "Identity", cadImage);
                    }

                    // Process block definitions using ValuesTyped as required
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        Console.WriteLine($"Block Definition, Entity Count: {block.Entities.Count}");

                        foreach (CadEntityBase blockEntity in block.Entities)
                        {
                            ProcessEntity(blockEntity, "Identity", cadImage);
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

    static void ProcessEntity(CadEntityBase entity, string parentMatrix, CadImage cadImage)
    {
        string typeName = entity.GetType().Name;
        string matrixInfo = parentMatrix;

        if (entity is CadInsertObject insert)
        {
            matrixInfo = $"ScaleX={insert.ScaleX}, ScaleY={insert.ScaleY}, ScaleZ={insert.ScaleZ}, Rotation={insert.RotationAngle}, InsertionPoint=({insert.InsertionPoint.X},{insert.InsertionPoint.Y},{insert.InsertionPoint.Z})";

            Console.WriteLine($"Entity Type: {typeName}, Transformation: {matrixInfo}");

            if (cadImage != null && cadImage.BlockEntities.TryGetValue(insert.Name, out CadBlockEntity referencedBlock))
            {
                foreach (CadEntityBase nestedEntity in referencedBlock.Entities)
                {
                    ProcessEntity(nestedEntity, matrixInfo, cadImage);
                }
            }
        }
        else
        {
            Console.WriteLine($"Entity Type: {typeName}, Transformation: {matrixInfo}");
        }
    }
}