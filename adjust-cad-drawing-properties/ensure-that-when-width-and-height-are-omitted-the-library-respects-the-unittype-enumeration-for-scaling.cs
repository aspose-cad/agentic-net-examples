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
            // Path to the CAD file (DWG or DXF)
            string cadFilePath = "sample.dwg";

            if (!File.Exists(cadFilePath))
            {
                Console.WriteLine($"File not found: {cadFilePath}");
                return;
            }

            try
            {
                // Load the CAD file using Aspose.CAD.Image.Load
                using (Image image = Image.Load(cadFilePath))
                {
                    // Cast the loaded image to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Inspect top‑level drawing entities
                    Console.WriteLine("Top‑level Entities:");
                    foreach (CadEntityBase topEntity in cadImage.Entities)
                    {
                        PrintEntityInfo(topEntity, "  ");
                    }

                    // Inspect block definitions
                    Console.WriteLine("\nBlock Definitions:");
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        Console.WriteLine($"Block Name: {block.Name}");
                        foreach (CadEntityBase blockEntity in block.Entities)
                        {
                            PrintEntityInfo(blockEntity, "  ");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing the CAD file: {ex.Message}");
            }
        }

        // Helper method to print information about a CAD entity
        private static void PrintEntityInfo(CadEntityBase entity, string indent)
        {
            // Use TypeName (enum) converted to string, or fallback to the CLR type name
            string typeName = entity.TypeName != null ? entity.TypeName.ToString() : entity.GetType().Name;
            Console.WriteLine($"{indent}Entity Type: {typeName}");

            // Layer name
            Console.WriteLine($"{indent}Layer: {entity.LayerName}");

            // Detailed handling for known entity types
            if (entity is CadLine line)
            {
                Console.WriteLine($"{indent}Line Start: ({line.FirstPoint.X}, {line.FirstPoint.Y}, {line.FirstPoint.Z})");
                Console.WriteLine($"{indent}Line End:   ({line.SecondPoint.X}, {line.SecondPoint.Y}, {line.SecondPoint.Z})");
            }
            else if (entity is CadLwPolyline polyline)
            {
                Console.WriteLine($"{indent}Polyline Points:");
                foreach (Cad2DPoint pt in polyline.Coordinates)
                {
                    Console.WriteLine($"{indent}  ({pt.X}, {pt.Y})");
                }
            }
            else if (entity is Cad3DFace face)
            {
                Console.WriteLine($"{indent}3D Face Corners:");
                Console.WriteLine($"{indent}  1: ({face.FirstCorner.X}, {face.FirstCorner.Y}, {face.FirstCorner.Z})");
                Console.WriteLine($"{indent}  2: ({face.SecondCorner.X}, {face.SecondCorner.Y}, {face.SecondCorner.Z})");
                Console.WriteLine($"{indent}  3: ({face.ThirdCorner.X}, {face.ThirdCorner.Y}, {face.ThirdCorner.Z})");
                Console.WriteLine($"{indent}  4: ({face.FourthCorner.X}, {face.FourthCorner.Y}, {face.FourthCorner.Z})");
            }
            else if (entity is CadInsertObject insert)
            {
                Console.WriteLine($"{indent}Insert Reference to Block: {insert.Name}");
                Console.WriteLine($"{indent}ScaleX: {insert.ScaleX}, ScaleY: {insert.ScaleY}, ScaleZ: {insert.ScaleZ}");
                Console.WriteLine($"{indent}Insertion Point: ({insert.InsertionPoint.X}, {insert.InsertionPoint.Y}, {insert.InsertionPoint.Z})");
            }
            else
            {
                // For other entity types, just output that no detailed handling is implemented
                Console.WriteLine($"{indent}No detailed handling for this entity type.");
            }
        }
    }
}
