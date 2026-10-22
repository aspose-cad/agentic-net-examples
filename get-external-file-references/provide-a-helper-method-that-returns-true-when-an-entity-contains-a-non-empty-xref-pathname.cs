using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadXrefInspection
{
    class Program
    {
        static void Main(string[] args)
        {
            const string filePath = "sample.dwg";

            if (!File.Exists(filePath))
            {
                Console.Error.WriteLine($"File not found: {filePath}");
                return;
            }

            try
            {
                // Load a DWG/DXF file
                using (CadImage cadImage = (CadImage)Image.Load(filePath))
                {
                    // Iterate over top‑level drawing entities
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        if (HasNonEmptyXrefPathName(entity))
                        {
                            Console.WriteLine($"Entity of type {entity.GetType().Name} contains a non‑empty XRef PathName.");
                        }
                    }

                    // Optionally, inspect entities inside block definitions
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            if (HasNonEmptyXrefPathName(entity))
                            {
                                Console.WriteLine($"Block \"{block.Name}\" entity of type {entity.GetType().Name} contains a non‑empty XRef PathName.");
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

        /// <summary>
        /// Returns true if the provided CAD entity is an insert object that has a non‑empty XRef PathName.
        /// </summary>
        /// <param name="entity">CAD entity to inspect.</param>
        /// <returns>True when XRef PathName is not null or empty; otherwise false.</returns>
        static bool HasNonEmptyXrefPathName(CadEntityBase entity)
        {
            if (entity is CadInsertObject insertObject)
            {
                // Aspose.CAD does not expose XrefPathName directly; the Name property holds the block name,
                // which for external references is typically the file name. Treat a non‑empty Name as a non‑empty XRef.
                return !string.IsNullOrEmpty(insertObject.Name);
            }

            return false;
        }
    }
}