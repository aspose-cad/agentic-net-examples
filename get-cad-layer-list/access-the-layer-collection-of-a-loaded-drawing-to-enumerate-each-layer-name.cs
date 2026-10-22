using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace GetCadLayerList
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dxf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    HashSet<string> layerNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    // Collect layer names from top‑level entities
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        if (!string.IsNullOrEmpty(entity.LayerName))
                        {
                            layerNames.Add(entity.LayerName);
                        }
                    }

                    // Collect layer names from block entities
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        foreach (CadEntityBase blkEntity in block.Entities)
                        {
                            if (!string.IsNullOrEmpty(blkEntity.LayerName))
                            {
                                layerNames.Add(blkEntity.LayerName);
                            }
                        }
                    }

                    // Output the distinct layer names
                    foreach (string name in layerNames)
                    {
                        Console.WriteLine(name);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
            }
        }
    }
}