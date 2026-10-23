using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Dwf;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.dwf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image image = Image.Load(inputPath))
            {
                if (image is not DwfImage dwfImage)
                {
                    Console.Error.WriteLine("The loaded file is not a DWF image.");
                    return;
                }

                // Retrieve layer names using the DWF model
                var layerNames = dwfImage.Layers.GetLayersNames();

                foreach (var name in layerNames)
                {
                    Console.WriteLine(name);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}