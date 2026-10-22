using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadLayerCompositionReport
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dwg"; // Placeholder path to the CAD file

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: File not found - {inputPath}");
                return;
            }

            try
            {
                using (Image cadImageBase = Image.Load(inputPath))
                {
                    CadImage cadImage = cadImageBase as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Error: Loaded file is not a supported CAD image.");
                        return;
                    }

                    // Dictionary: Layer name -> (Entity type name -> count)
                    Dictionary<string, Dictionary<string, int>> layerComposition = new Dictionary<string, Dictionary<string, int>>(StringComparer.OrdinalIgnoreCase);

                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        string layerName = entity.LayerName ?? "UnnamedLayer";
                        string entityType = entity.GetType().Name;

                        if (!layerComposition.TryGetValue(layerName, out Dictionary<string, int> typeCounts))
                        {
                            typeCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                            layerComposition[layerName] = typeCounts;
                        }

                        if (typeCounts.ContainsKey(entityType))
                        {
                            typeCounts[entityType]++;
                        }
                        else
                        {
                            typeCounts[entityType] = 1;
                        }
                    }

                    // Output the report
                    Console.WriteLine("Layer Composition Report:");
                    Console.WriteLine("--------------------------");

                    foreach (KeyValuePair<string, Dictionary<string, int>> layerEntry in layerComposition)
                    {
                        Console.WriteLine($"Layer: {layerEntry.Key}");
                        foreach (KeyValuePair<string, int> typeEntry in layerEntry.Value)
                        {
                            Console.WriteLine($"  {typeEntry.Key}: {typeEntry.Value}");
                        }
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}