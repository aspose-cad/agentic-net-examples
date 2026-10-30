using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Dwf;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                if (image is not DwfImage dwfImage)
                {
                    Console.Error.WriteLine("Failed to load the DWF file as a DwfImage.");
                    return;
                }

                var layersList = dwfImage.Layers;
                if (layersList == null || layersList.Count == 0)
                {
                    Console.WriteLine("No layers found in the drawing.");
                    return;
                }

                var layerNames = layersList.GetLayersNames();
                if (layerNames == null || layerNames.Count == 0)
                {
                    Console.WriteLine("No layer names retrieved.");
                    return;
                }

                Console.WriteLine("Layer information:");
                foreach (var name in layerNames)
                {
                    Console.WriteLine($"- Name: {name}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing the DWF file: {ex.Message}");
        }
    }
}