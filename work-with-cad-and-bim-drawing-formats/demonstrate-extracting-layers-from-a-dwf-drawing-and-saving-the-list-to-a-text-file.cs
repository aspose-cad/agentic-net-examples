using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Dwf;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwf";
        const string outputPath = "layers.txt";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                DwfImage dwfImage = image as DwfImage;
                if (dwfImage == null)
                {
                    Console.Error.WriteLine("The provided file is not a DWF drawing.");
                    return;
                }

                // Extract layer names using the DWF model
                List<string> layerNames = dwfImage.Layers.GetLayersNames();

                File.WriteAllLines(outputPath, layerNames);
                Console.WriteLine($"Extracted {layerNames.Count} layer(s) to '{outputPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}