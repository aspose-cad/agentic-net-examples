using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadBlockVerification
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputPath = "input.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)image;

                    var blockDict = cadImage.BlockEntities;

                    HashSet<string> nameSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    bool duplicateFound = false;

                    foreach (DictionaryEntry entry in blockDict)
                    {
                        string blockName = entry.Key as string;
                        if (blockName == null) continue;

                        if (!nameSet.Add(blockName))
                        {
                            duplicateFound = true;
                            Console.WriteLine($"Duplicate block name found: {blockName}");
                        }
                    }

                    if (!duplicateFound)
                    {
                        Console.WriteLine("No duplicate block names found.");
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