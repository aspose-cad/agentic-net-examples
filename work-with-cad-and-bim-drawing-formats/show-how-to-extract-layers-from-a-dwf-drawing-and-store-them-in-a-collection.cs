using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Dwf;

namespace ExtractDwfLayers
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source DWF file
                string sourcePath = "input.dwf";

                // Verify the input file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Input file not found: {sourcePath}");
                    return;
                }

                // Load the DWF drawing
                using (Image image = Image.Load(sourcePath))
                {
                    // Cast to DwfImage to access DWF-specific members
                    DwfImage dwf = image as DwfImage;
                    if (dwf == null)
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to DwfImage.");
                        return;
                    }

                    // Extract layer names
                    List<string> layerNames = new List<string>();
                    try
                    {
                        // The DwfLayersList provides GetLayersNames() according to the API
                        var names = dwf.Layers.GetLayersNames();
                        if (names != null)
                        {
                            layerNames.AddRange(names);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error retrieving layer names: {ex.Message}");
                        return;
                    }

                    // Store layers in a collection (already in layerNames)
                    Console.WriteLine("Extracted layer names:");
                    foreach (var name in layerNames)
                    {
                        Console.WriteLine($"- {name}");
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