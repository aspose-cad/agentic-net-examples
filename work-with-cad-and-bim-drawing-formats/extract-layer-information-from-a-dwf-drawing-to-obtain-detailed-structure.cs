using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Dwf;
using Aspose.CAD.FileFormats.Dwf.Whip.Objects.Drawable;

namespace AsposeCadLayerExtraction
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "sample.dwf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                using (Image img = Image.Load(inputPath))
                {
                    if (img is not DwfImage dwfImage)
                    {
                        Console.Error.WriteLine("The provided file is not a DWF drawing.");
                        return;
                    }

                    // Retrieve layer names
                    List<string> layerNames = dwfImage.Layers.GetLayersNames();

                    // Prepare dictionary to hold entity counts per layer
                    Dictionary<string, int> layerEntityCount = new(StringComparer.OrdinalIgnoreCase);
                    foreach (string name in layerNames)
                    {
                        layerEntityCount[name] = 0;
                    }

                    // Iterate through pages and their drawable entities
                    foreach (DwfPage page in dwfImage.Pages)
                    {
                        foreach (var drawable in page.Entities)
                        {
                            // Use dynamic to access LayerIndex if present
                            try
                            {
                                int layerIdx = ((dynamic)drawable).LayerIndex;
                                if (layerIdx >= 0 && layerIdx < layerNames.Count)
                                {
                                    string layerName = layerNames[layerIdx];
                                    layerEntityCount[layerName]++;
                                }
                            }
                            catch
                            {
                                // Entity does not expose LayerIndex; ignore for counting
                            }
                        }
                    }

                    Console.WriteLine("Layer information extracted from the DWF drawing:");
                    foreach (var kvp in layerEntityCount)
                    {
                        Console.WriteLine($"Layer: {kvp.Key}, Entity Count: {kvp.Value}");
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