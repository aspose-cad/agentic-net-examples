using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Dwf;

class Program
{
    static void Main()
    {
        string sourcePath = "input.dwf";

        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(sourcePath))
            {
                if (image is DwfImage dwfImage)
                {
                    Console.WriteLine("Layers in the DWF file:");
                    // Get layer names without modifying the original file
                    var layerNames = dwfImage.Layers.GetLayersNames();
                    foreach (var name in layerNames)
                    {
                        Console.WriteLine("- " + name);
                    }
                }
                else
                {
                    Console.Error.WriteLine("Loaded image is not a DWF image.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}