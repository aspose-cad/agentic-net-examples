using System;
using System.Collections;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadEntityEnumeration
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the CAD file
            string sourcePath = "sample.dwg";

            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"File not found: {sourcePath}");
                return;
            }

            try
            {
                // Load the CAD file
                using (Image image = Image.Load(sourcePath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Enumerate top-level entities
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        PrintEntityInfo(entity);
                    }

                    // Enumerate block entities (if any)
                    foreach (DictionaryEntry blockEntry in cadImage.BlockEntities)
                    {
                        if (blockEntry.Value is CadBlockEntity blockEntity)
                        {
                            Console.WriteLine($"Block: {blockEntry.Key}");
                            foreach (CadEntityBase entity in blockEntity.Entities)
                            {
                                PrintEntityInfo(entity, indent: "  ");
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

        private static void PrintEntityInfo(CadEntityBase entity, string indent = "")
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
                foreach (Cad3DPoint point in polyline.Coordinates)
                {
                    Console.WriteLine($"{indent}    ({point.X}, {point.Y}, {point.Z})");
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
                Console.WriteLine($"{indent}  Inserted block: {insert.Name}");
                Console.WriteLine($"{indent}    Scale: ({insert.ScaleX}, {insert.ScaleY}, {insert.ScaleZ})");
                Console.WriteLine($"{indent}    Rotation: {insert.RotationAngle}");
            }
        }
    }
}