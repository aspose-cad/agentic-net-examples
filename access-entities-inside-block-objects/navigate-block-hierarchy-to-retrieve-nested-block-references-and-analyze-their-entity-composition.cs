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
                    Console.WriteLine($"File not found: {sourcePath}");
                    return;
                }

                // Load the CAD file
                using (Image image = Image.Load(sourcePath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    Console.WriteLine("Top‑level entities:");
                    // Inspect top‑level entities for block references
                    foreach (CadEntityBase topEntity in cadImage.Entities)
                    {
                        if (topEntity is CadInsertObject insert)
                        {
                            Console.WriteLine($"Insert reference to block \"{insert.Name}\" at level 0");
                            ProcessBlock(cadImage, insert.Name, 1);
                        }
                        else
                        {
                            Console.WriteLine($"Entity type: {topEntity.GetType().Name}");
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

        // Recursively process a block definition and its nested inserts
        private static void ProcessBlock(CadImage cadImage, string blockName, int level)
        {
            // Find the block definition by name
            CadBlockEntity blockDefinition = null;
            foreach (CadBlockEntity blk in cadImage.BlockEntities.ValuesTyped)
            {
                if (blk.Name.Equals(blockName, StringComparison.OrdinalIgnoreCase))
                {
                    blockDefinition = blk;
                    break;
                }
            }

            if (blockDefinition == null)
            {
                Console.WriteLine($"{new string(' ', level * 2)}Block \"{blockName}\" not found.");
                return;
            }

            Console.WriteLine($"{new string(' ', level * 2)}Analyzing block \"{blockDefinition.Name}\" (Level {level})");

            // Iterate over entities inside the block
            foreach (CadEntityBase entity in blockDefinition.Entities)
            {
                switch (entity)
                {
                    case CadLine line:
                        Console.WriteLine($"{new string(' ', (level + 1) * 2)}Line from ({line.FirstPoint.X}, {line.FirstPoint.Y}, {line.FirstPoint.Z}) to ({line.SecondPoint.X}, {line.SecondPoint.Y}, {line.SecondPoint.Z})");
                        break;

                    case CadLwPolyline polyline:
                        Console.WriteLine($"{new string(' ', (level + 1) * 2)}Lightweight polyline with {polyline.Coordinates.Count} points");
                        break;

                    case Cad3DFace face:
                        Console.WriteLine($"{new string(' ', (level + 1) * 2)}3D Face corners: ({face.FirstCorner.X}, {face.FirstCorner.Y}, {face.FirstCorner.Z}), ({face.SecondCorner.X}, {face.SecondCorner.Y}, {face.SecondCorner.Z}), ({face.ThirdCorner.X}, {face.ThirdCorner.Y}, {face.ThirdCorner.Z}), ({face.FourthCorner.X}, {face.FourthCorner.Y}, {face.FourthCorner.Z})");
                        break;

                    case CadInsertObject nestedInsert:
                        Console.WriteLine($"{new string(' ', (level + 1) * 2)}Insert reference to block \"{nestedInsert.Name}\" at level {level}");
                        // Recursively process the nested block
                        ProcessBlock(cadImage, nestedInsert.Name, level + 1);
                        break;

                    default:
                        Console.WriteLine($"{new string(' ', (level + 1) * 2)}Entity type: {entity.GetType().Name}");
                        break;
                }
            }
        }
    }
}