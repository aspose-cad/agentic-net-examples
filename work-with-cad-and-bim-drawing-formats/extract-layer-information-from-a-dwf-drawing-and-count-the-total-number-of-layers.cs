using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Dwf;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                DwfImage dwf = image as DwfImage;
                if (dwf == null)
                {
                    Console.Error.WriteLine("The file is not a DWF drawing.");
                    return;
                }

                var layerNames = dwf.Layers.GetLayersNames();
                int totalLayers = 0;

                if (layerNames != null)
                {
                    foreach (var name in layerNames)
                    {
                        Console.WriteLine($"Layer: {name}");
                        totalLayers++;
                    }
                }

                Console.WriteLine($"Total number of layers: {totalLayers}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}