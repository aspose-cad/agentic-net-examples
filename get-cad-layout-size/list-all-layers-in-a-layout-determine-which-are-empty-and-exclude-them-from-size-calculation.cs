using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadLayerAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the CAD file (DWG/DXF)
                string inputPath = "input.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD drawing and cast to CadImage
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Dictionary to track whether each layer contains any entities
                    Dictionary<string, bool> layerHasEntities = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

                    // Initialize dictionary entries for all layers as empty
                    foreach (CadLayerTable layerRecord in cadImage.Layers)
                    {
                        if (!layerHasEntities.ContainsKey(layerRecord.Name))
                        {
                            layerHasEntities[layerRecord.Name] = false;
                        }
                    }

                    // Examine top‑level entities and mark their layers as non‑empty
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        string layerName = entity.LayerName;
                        if (layerHasEntities.ContainsKey(layerName))
                        {
                            layerHasEntities[layerName] = true;
                        }
                    }

                    // Examine entities inside block definitions as well
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            string layerName = entity.LayerName;
                            if (layerHasEntities.ContainsKey(layerName))
                            {
                                layerHasEntities[layerName] = true;
                            }
                        }
                    }

                    // Output the list of layers and indicate which are empty
                    Console.WriteLine("Layers in the drawing:");
                    foreach (KeyValuePair<string, bool> kvp in layerHasEntities)
                    {
                        Console.WriteLine($"{kvp.Key} - {(kvp.Value ? "Non‑empty" : "Empty")}");
                    }

                    // Example size‑related calculation: count entities that belong to non‑empty layers
                    int totalEntityCount = 0;

                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        if (layerHasEntities.TryGetValue(entity.LayerName, out bool isNonEmpty) && isNonEmpty)
                        {
                            totalEntityCount++;
                        }
                    }

                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            if (layerHasEntities.TryGetValue(entity.LayerName, out bool isNonEmpty) && isNonEmpty)
                            {
                                totalEntityCount++;
                            }
                        }
                    }

                    Console.WriteLine($"Total entity count (excluding empty layers): {totalEntityCount}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}