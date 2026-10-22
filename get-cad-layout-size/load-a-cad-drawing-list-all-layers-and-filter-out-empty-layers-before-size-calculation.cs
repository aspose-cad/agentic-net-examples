using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadLayerInspection
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the CAD drawing
                string inputPath = "input.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD‑specific members
                    CadImage cadImage = (CadImage)image;

                    // List to hold names of layers that contain at least one entity
                    List<string> nonEmptyLayerNames = new List<string>();

                    // Iterate over each layer
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        string layerName = layer.Name;
                        bool hasEntity = false;

                        // Check top‑level entities
                        foreach (CadEntityBase entity in cadImage.Entities)
                        {
                            if (entity.LayerName == layerName)
                            {
                                hasEntity = true;
                                break;
                            }
                        }

                        // If not found, check entities inside block definitions
                        if (!hasEntity)
                        {
                            foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                            {
                                foreach (CadEntityBase entity in block.Entities)
                                {
                                    if (entity.LayerName == layerName)
                                    {
                                        hasEntity = true;
                                        break;
                                    }
                                }
                                if (hasEntity) break;
                            }
                        }

                        // Add to the result list if the layer is not empty
                        if (hasEntity)
                        {
                            nonEmptyLayerNames.Add(layerName);
                        }
                    }

                    // Output the non‑empty layers
                    Console.WriteLine("Non‑empty layers:");
                    foreach (string name in nonEmptyLayerNames)
                    {
                        Console.WriteLine($"- {name}");
                    }

                    // Example size calculation: total number of entities in non‑empty layers
                    int totalEntityCount = 0;

                    // Count entities on non‑empty layers at top level
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        if (nonEmptyLayerNames.Contains(entity.LayerName))
                            totalEntityCount++;
                    }

                    // Count entities inside block definitions
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            if (nonEmptyLayerNames.Contains(entity.LayerName))
                                totalEntityCount++;
                        }
                    }

                    Console.WriteLine($"Total number of entities in non‑empty layers: {totalEntityCount}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}