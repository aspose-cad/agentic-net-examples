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
                // Path to the CAD file (DWG or DXF)
                string cadFilePath = "sample.dwg";

                // Guard against missing file
                if (!File.Exists(cadFilePath))
                {
                    Console.WriteLine($"File not found: {cadFilePath}");
                    return;
                }

                // Load the CAD file and cast to CadImage
                using (Image image = Image.Load(cadFilePath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("Failed to load CAD image.");
                        return;
                    }

                    // Enumerate top‑level drawing entities
                    foreach (CadEntityBase topEntity in cadImage.Entities)
                    {
                        ProcessEntity(topEntity, cadImage);
                    }

                    // Enumerate block definitions safely
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        if (block == null) continue;

                        Console.WriteLine($"Block Definition: {block.Name}");
                        foreach (CadEntityBase blockEntity in block.Entities)
                        {
                            ProcessEntity(blockEntity, cadImage);
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

        // Process a single CAD entity, handling inserts and other types
        private static void ProcessEntity(CadEntityBase entity, CadImage cadImage)
        {
            // Log the entity type
            Console.WriteLine($"Entity Type: {entity.GetType().Name}");

            // Handle line entities
            if (entity is CadLine line)
            {
                Console.WriteLine($"  Line from ({line.FirstPoint.X}, {line.FirstPoint.Y}, {line.FirstPoint.Z}) " +
                                  $"to ({line.SecondPoint.X}, {line.SecondPoint.Y}, {line.SecondPoint.Z})");
            }
            // Handle lightweight polyline entities
            else if (entity is CadLwPolyline lwPolyline)
            {
                Console.WriteLine("  LwPolyline points:");
                foreach (Cad2DPoint pt in lwPolyline.Coordinates)
                {
                    Console.WriteLine($"    ({pt.X}, {pt.Y})");
                }
            }
            // Handle 3D face entities
            else if (entity is Cad3DFace face)
            {
                Console.WriteLine($"  3DFace corners: " +
                                  $"({face.FirstCorner.X}, {face.FirstCorner.Y}, {face.FirstCorner.Z}), " +
                                  $"({face.SecondCorner.X}, {face.SecondCorner.Y}, {face.SecondCorner.Z}), " +
                                  $"({face.ThirdCorner.X}, {face.ThirdCorner.Y}, {face.ThirdCorner.Z}), " +
                                  $"({face.FourthCorner.X}, {face.FourthCorner.Y}, {face.FourthCorner.Z})");
            }
            // Handle block insert (reference) entities
            else if (entity is CadInsertObject insert)
            {
                string blockName = insert.Name;
                Console.WriteLine($"  Insert reference to block: {blockName}");

                // Find the referenced block definition by iterating safely
                CadBlockEntity referencedBlock = null;
                foreach (CadBlockEntity b in cadImage.BlockEntities.ValuesTyped)
                {
                    if (b != null && b.Name == blockName)
                    {
                        referencedBlock = b;
                        break;
                    }
                }

                if (referencedBlock == null)
                {
                    Console.WriteLine("    Warning: Referenced block not found. Skipping.");
                    return;
                }

                Console.WriteLine($"    Entering block '{referencedBlock.Name}' contents:");
                foreach (CadEntityBase innerEntity in referencedBlock.Entities)
                {
                    // Recursively process entities inside the block
                    ProcessEntity(innerEntity, cadImage);
                }
            }
            else
            {
                // Other entity types can be handled here
                Console.WriteLine("  (Other entity type not explicitly handled)");
            }
        }
    }
}