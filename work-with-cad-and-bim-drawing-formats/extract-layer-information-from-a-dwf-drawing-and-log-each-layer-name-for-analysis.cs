using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Dwf;

class Program
{
    static void Main()
    {
        // Path to the DWF file
        string sourcePath = "input.dwf";

        // Verify the input file exists
        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        try
        {
            // Load the drawing (DWF loads as DwfImage)
            using (Image image = Image.Load(sourcePath))
            {
                if (image is DwfImage dwfImage)
                {
                    // Extract layer names
                    IEnumerable<string> layerNames = dwfImage.Layers.GetLayersNames();

                    if (layerNames != null)
                    {
                        foreach (string name in layerNames)
                        {
                            Console.WriteLine($"Layer: {name}");
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine("No layers found in the DWF file.");
                    }
                }
                else
                {
                    Console.Error.WriteLine("The loaded file is not a DWF image; layer extraction is unavailable.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}