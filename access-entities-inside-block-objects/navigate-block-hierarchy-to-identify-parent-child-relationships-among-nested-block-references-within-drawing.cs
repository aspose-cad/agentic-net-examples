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
            Console.WriteLine($"File not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                var blockHierarchy = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

                foreach (CadBlockEntity blockEntity in cadImage.BlockEntities.ValuesTyped)
                {
                    string parentBlockName = blockEntity.Name;

                    if (!blockHierarchy.ContainsKey(parentBlockName))
                    {
                        blockHierarchy[parentBlockName] = new List<string>();
                    }

                    foreach (CadEntityBase entity in blockEntity.Entities)
                    {
                        if (entity is CadInsertObject insertObject)
                        {
                            string childBlockName = insertObject.Name;
                            blockHierarchy[parentBlockName].Add(childBlockName);
                        }
                    }
                }

                foreach (KeyValuePair<string, List<string>> kvp in blockHierarchy)
                {
                    Console.WriteLine($"Block \"{kvp.Key}\" contains the following nested block references:");
                    foreach (string childName in kvp.Value)
                    {
                        Console.WriteLine($"  -> {childName}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}