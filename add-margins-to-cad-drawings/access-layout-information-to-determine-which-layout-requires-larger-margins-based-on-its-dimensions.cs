using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image cadImage = Image.Load(inputPath))
            {
                CadImage cad = cadImage as CadImage;
                if (cad == null)
                {
                    Console.Error.WriteLine("Failed to load CAD image.");
                    return;
                }

                // Prefer layouts if they exist; otherwise fall back to block entities.
                if (cad.Layouts != null && cad.Layouts.Count > 0)
                {
                    string layoutWithLargestMargin = null;
                    int maxEntityCount = -1;

                    foreach (KeyValuePair<string, CadLayout> kvp in cad.Layouts)
                    {
                        string layoutName = kvp.Key;
                        CadLayout layout = kvp.Value;

                        int entityCount = layout?.Entities?.Count() ?? 0;

                        if (entityCount > maxEntityCount)
                        {
                            maxEntityCount = entityCount;
                            layoutWithLargestMargin = layoutName;
                        }
                    }

                    if (layoutWithLargestMargin != null)
                    {
                        Console.WriteLine($"Layout requiring larger margins (most entities): {layoutWithLargestMargin}");
                        Console.WriteLine($"Entity count used as proxy: {maxEntityCount}");
                    }
                    else
                    {
                        Console.WriteLine("Unable to determine layout dimensions.");
                    }
                }
                else if (cad.BlockEntities != null && cad.BlockEntities.Count > 0)
                {
                    string blockWithLargestMargin = null;
                    int maxEntityCount = -1;

                    foreach (KeyValuePair<string, CadBlockEntity> kvp in cad.BlockEntities)
                    {
                        string blockName = kvp.Key;
                        CadBlockEntity block = kvp.Value;

                        int entityCount = block?.Entities?.Count() ?? 0;

                        if (entityCount > maxEntityCount)
                        {
                            maxEntityCount = entityCount;
                            blockWithLargestMargin = blockName;
                        }
                    }

                    if (blockWithLargestMargin != null)
                    {
                        Console.WriteLine($"Block requiring larger margins (most entities): {blockWithLargestMargin}");
                        Console.WriteLine($"Entity count used as proxy: {maxEntityCount}");
                    }
                    else
                    {
                        Console.WriteLine("Unable to determine block dimensions.");
                    }
                }
                else
                {
                    Console.WriteLine("No layouts or block entities found in the drawing.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
