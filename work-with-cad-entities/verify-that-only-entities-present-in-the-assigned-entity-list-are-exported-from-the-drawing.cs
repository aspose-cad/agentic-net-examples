using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image sourceImage = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)sourceImage;

                HashSet<string> allowedEntityTypes = new HashSet<string>
                {
                    nameof(CadLine),
                    nameof(CadLwPolyline),
                    nameof(Cad3DFace)
                };

                List<CadEntityBase> disallowedEntities = new List<CadEntityBase>();

                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    string typeName = entity.GetType().Name;
                    if (!allowedEntityTypes.Contains(typeName))
                    {
                        disallowedEntities.Add(entity);
                    }
                }

                if (disallowedEntities.Count == 0)
                {
                    Console.WriteLine("Verification passed: only allowed entities are present for export.");
                }
                else
                {
                    Console.WriteLine("Verification failed: the following entities are not in the allowed list:");
                    foreach (CadEntityBase entity in disallowedEntities)
                    {
                        Console.WriteLine($"- {entity.GetType().Name}");
                    }
                }

                // Note: Aspose.CAD does not provide a direct API to create a new CAD file from a filtered set of entities.
                // Exporting only the allowed entities would require additional SDK capabilities not covered here.
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}