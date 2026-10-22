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
                // Path to the CAD file (DWG/DXF)
                string filePath = "sample.dwg";

                // Guard against missing file
                if (!File.Exists(filePath))
                {
                    Console.Error.WriteLine($"File not found: {filePath}");
                    return;
                }

                // Load the CAD file and cast to CadImage
                using (Image image = Image.Load(filePath))
                {
                    CadImage cadImage = (CadImage)image;

                    Console.WriteLine("Processing top‑level entities...");
                    foreach (CadEntityBase topEntity in cadImage.Entities)
                    {
                        ProcessEntity(topEntity, cadImage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }

        // Recursively process an entity; if it is an insert, retrieve its block definition
        private static void ProcessEntity(CadEntityBase entity, CadImage cadImage)
        {
            if (entity is CadInsertObject insert)
            {
                Console.WriteLine($"Insert found: Name = {insert.Name}, ScaleX = {insert.ScaleX}, ScaleY = {insert.ScaleY}, ScaleZ = {insert.ScaleZ}");

                // Locate the block definition that matches the insert's name
                CadBlockEntity blockDef = FindBlockDefinition(insert.Name, cadImage);
                if (blockDef != null)
                {
                    Console.WriteLine($"  Block definition '{blockDef.Name}' contains {blockDef.Entities.Count} entities.");
                    // Recursively process entities inside the block definition
                    foreach (CadEntityBase innerEntity in blockDef.Entities)
                    {
                        ProcessEntity(innerEntity, cadImage);
                    }
                }
                else
                {
                    Console.WriteLine($"  No block definition found for insert name '{insert.Name}'.");
                }
            }
            else
            {
                // Log other entity types
                Console.WriteLine($"Entity type: {entity.GetType().Name}");
            }
        }

        // Helper to find a block definition by name within the CadImage
        private static CadBlockEntity FindBlockDefinition(string blockName, CadImage cadImage)
        {
            foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
            {
                if (block.Name == blockName)
                {
                    return block;
                }
            }
            return null;
        }
    }
}