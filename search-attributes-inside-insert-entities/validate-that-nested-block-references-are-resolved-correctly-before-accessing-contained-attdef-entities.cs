using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadBlockInspection
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the CAD file (DWG/DXF). Use first argument if provided.
                string cadFilePath = args.Length > 0 ? args[0] : "sample.dwg";

                // Verify that the file exists before attempting to load it.
                if (!File.Exists(cadFilePath))
                {
                    Console.Error.WriteLine($"File not found: {cadFilePath}");
                    return;
                }

                // Load the CAD drawing.
                using (Image image = Image.Load(cadFilePath))
                {
                    // Cast to CadImage to access CAD object model.
                    CadImage cadImage = (CadImage)image;

                    // Iterate over all block definitions in the drawing.
                    foreach (CadBlockEntity blockDefinition in cadImage.BlockEntities.ValuesTyped)
                    {
                        Console.WriteLine($"Inspecting Block: {blockDefinition.Name}");

                        // Examine each entity inside the block definition.
                        foreach (CadEntityBase entity in blockDefinition.Entities)
                        {
                            // Look for block insert references (nested blocks).
                            if (entity is CadInsertObject insertObject)
                            {
                                Console.WriteLine($"  Found Insert Reference to Block: {insertObject.Name}");

                                // Resolve the referenced block definition without using LINQ.
                                CadBlockEntity referencedBlock = null;
                                foreach (CadBlockEntity blk in cadImage.BlockEntities.ValuesTyped)
                                {
                                    if (blk.Name == insertObject.Name)
                                    {
                                        referencedBlock = blk;
                                        break;
                                    }
                                }

                                if (referencedBlock == null)
                                {
                                    Console.WriteLine($"    Warning: Referenced block '{insertObject.Name}' not found.");
                                    continue;
                                }

                                // After resolution, search for ATTDEF (attribute definition) entities inside the referenced block.
                                foreach (CadEntityBase nestedEntity in referencedBlock.Entities)
                                {
                                    // Use TypeName comparison to identify attribute definitions without relying on a missing class.
                                    if (nestedEntity.TypeName != null && nestedEntity.TypeName.ToString().Equals("ATTDEF", StringComparison.OrdinalIgnoreCase))
                                    {
                                        dynamic attDef = nestedEntity;
                                        Console.WriteLine($"    ATTDEF found in block '{referencedBlock.Name}':");
                                        Console.WriteLine($"      Tag: {attDef.Tag}");
                                        Console.WriteLine($"      Prompt: {attDef.Prompt}");
                                        Console.WriteLine($"      Default Value: {attDef.DefaultValue}");
                                    }
                                }
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
    }
}