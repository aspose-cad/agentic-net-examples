using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

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
                if (image is not CadImage cadImage)
                {
                    Console.Error.WriteLine("The loaded file is not a CAD image.");
                    return;
                }

                var layerNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var entity in cadImage.Entities)
                {
                    if (entity?.LayerName != null)
                    {
                        layerNames.Add(entity.LayerName);
                    }
                }

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