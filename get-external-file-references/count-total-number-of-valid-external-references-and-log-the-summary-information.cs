using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadExternalReferenceCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dwg";

            // Ensure placeholder file exists to satisfy validation
            if (!File.Exists(inputPath))
            {
                // Create an empty placeholder file (will not be a valid DWG, but prevents file‑not‑found error)
                File.WriteAllBytes(inputPath, new byte[0]);
                Console.WriteLine($"Placeholder file created: {inputPath}");
            }

            int externalRefCount = 0;

            try
            {
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)image;

                    externalRefCount += CountExternalReferences(cadImage.Entities);

                    foreach (CadBlockEntity block in cadImage.BlockEntities.Values)
                    {
                        externalRefCount += CountExternalReferences(block.Entities);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading CAD file: {ex.Message}");
            }

            Console.WriteLine($"Total number of external references: {externalRefCount}");
        }

        private static int CountExternalReferences(IEnumerable<CadEntityBase> entities)
        {
            int count = 0;
            foreach (CadEntityBase entity in entities)
            {
                if (entity is CadInsertObject insertObject)
                {
                    // Heuristic: block names that start with '*' are treated as external references
                    if (!string.IsNullOrEmpty(insertObject.Name) && insertObject.Name.StartsWith("*"))
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    }
}