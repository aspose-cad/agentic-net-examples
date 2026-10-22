using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadBlockInspection
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the DWG/DXF file
                string sourcePath = "sample.dwg";

                // Guard against missing file
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"File not found: {sourcePath}");
                    return;
                }

                // Load the CAD file
                using (Image image = Image.Load(sourcePath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Process top‑level entities (including block inserts)
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        ProcessEntity(cadImage, entity, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }

        // Recursively process an entity and, if it is a block insert, its nested entities
        private static void ProcessEntity(CadImage cadImage, CadEntityBase entity, int level)
        {
            string indent = new string(' ', level * 2);

            switch (entity)
            {
                case CadLine line:
                    Console.WriteLine($"{indent}Line: ({line.FirstPoint.X}, {line.FirstPoint.Y}, {line.FirstPoint.Z}) -> ({line.SecondPoint.X}, {line.SecondPoint.Y}, {line.SecondPoint.Z})");
                    break;

                case CadLwPolyline polyline:
                    Console.WriteLine($"{indent}Lightweight Polyline with {polyline.Coordinates.Count} points:");
                    foreach (Cad2DPoint pt in polyline.Coordinates)
                    {
                        Console.WriteLine($"{indent}  Point: ({pt.X}, {pt.Y})");
                    }
                    break;

                case Cad3DFace face:
                    Console.WriteLine($"{indent}3D Face corners:");
                    Console.WriteLine($"{indent}  1: ({face.FirstCorner.X}, {face.FirstCorner.Y}, {face.FirstCorner.Z})");
                    Console.WriteLine($"{indent}  2: ({face.SecondCorner.X}, {face.SecondCorner.Y}, {face.SecondCorner.Z})");
                    Console.WriteLine($"{indent}  3: ({face.ThirdCorner.X}, {face.ThirdCorner.Y}, {face.ThirdCorner.Z})");
                    Console.WriteLine($"{indent}  4: ({face.FourthCorner.X}, {face.FourthCorner.Y}, {face.FourthCorner.Z})");
                    break;

                case CadInsertObject insert:
                    Console.WriteLine($"{indent}Block Insert: {insert.Name}");
                    // Find the block definition by name
                    CadBlockEntity blockDef = FindBlockDefinition(cadImage, insert.Name);
                    if (blockDef != null)
                    {
                        // Process entities inside the block definition
                        foreach (CadEntityBase innerEntity in blockDef.Entities)
                        {
                            ProcessEntity(cadImage, innerEntity, level + 1);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{indent}  Block definition not found.");
                    }
                    break;

                default:
                    Console.WriteLine($"{indent}Other entity type: {entity.GetType().Name}");
                    break;
            }
        }

        // Helper to locate a block definition by its name
        private static CadBlockEntity FindBlockDefinition(CadImage cadImage, string blockName)
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
}