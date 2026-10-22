using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadAttributeDefinitionInspector
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the DWG/DXF file
            string sourcePath = "input.dwg";

            // Guard against missing input file
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Input file not found: {sourcePath}");
                return;
            }

            try
            {
                // Load the CAD drawing
                using (Image image = Image.Load(sourcePath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // List to collect found attribute definition entities (generic base type)
                    List<CadEntityBase> foundAttDefs = new List<CadEntityBase>();

                    // Process top‑level entities
                    ProcessEntities(cadImage.Entities, cadImage, foundAttDefs);

                    // Output results
                    Console.WriteLine($"Found {foundAttDefs.Count} attribute definition(s):");
                    foreach (CadEntityBase attDef in foundAttDefs)
                    {
                        Console.WriteLine($"Handle: {attDef.ObjectHandle}, Type: {attDef.TypeName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }

        // Recursively processes a collection of CAD entities
        private static void ProcessEntities(IEnumerable<CadEntityBase> entities, CadImage cadImage, List<CadEntityBase> foundAttDefs)
        {
            foreach (CadEntityBase entity in entities)
            {
                // Identify attribute definition entities by their type name (ATTDEF)
                if (entity.TypeName.ToString().Equals("AttDef", StringComparison.OrdinalIgnoreCase))
                {
                    foundAttDefs.Add(entity);
                }
                // If the entity is an insert (block reference), process the referenced block recursively
                else if (entity is CadInsertObject insert)
                {
                    // Retrieve the block definition by matching the insert name
                    var block = cadImage.BlockEntities.ValuesTyped
                                 .FirstOrDefault(b => b.Name.Equals(insert.Name, StringComparison.OrdinalIgnoreCase));

                    if (block != null)
                    {
                        // Process entities inside the block definition
                        ProcessEntities(block.Entities, cadImage, foundAttDefs);
                    }
                }
                // Other entity types are ignored for this task
            }
        }
    }
}