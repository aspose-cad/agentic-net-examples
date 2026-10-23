using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Dwf;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source DWF file
            string sourcePath = "sample.dwf";

            // Verify the file exists before attempting to load
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"File not found: {sourcePath}");
                return;
            }

            // Load the drawing
            using (Image image = Image.Load(sourcePath))
            {
                // DWF drawings are represented by DwfImage
                if (image is DwfImage dwfImage)
                {
                    // Extract layer names
                    IList<string> layerNames = dwfImage.Layers.GetLayersNames();

                    if (layerNames != null && layerNames.Count > 0)
                    {
                        Console.WriteLine("Layers in the DWF drawing:");
                        foreach (string name in layerNames)
                        {
                            Console.WriteLine($"- {name}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No layers found in the drawing.");
                    }
                }
                else
                {
                    Console.WriteLine("The provided file is not a DWF drawing.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}