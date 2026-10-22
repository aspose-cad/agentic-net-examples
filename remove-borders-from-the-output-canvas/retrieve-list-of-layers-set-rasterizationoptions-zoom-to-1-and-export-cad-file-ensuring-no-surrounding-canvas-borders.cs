using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                if (image is not CadImage cadImage)
                {
                    Console.Error.WriteLine("Loaded image is not a CAD image.");
                    return;
                }

                // Retrieve distinct layer names from entities (reflection used for safety)
                var layerNames = new HashSet<string>();
                foreach (var entity in cadImage.Entities)
                {
                    var prop = entity.GetType().GetProperty("LayerName");
                    if (prop != null)
                    {
                        var value = prop.GetValue(entity) as string;
                        if (!string.IsNullOrEmpty(value))
                            layerNames.Add(value);
                    }
                }

                Console.WriteLine("Layers in the drawing:");
                foreach (var name in layerNames)
                {
                    Console.WriteLine(name);
                }

                var rasterOptions = new CadRasterizationOptions
                {
                    Zoom = 1,
                    // Remove surrounding canvas borders if the property exists
                    // (property may be named RemoveBorders or similar in different versions)
                };

                // Attempt to set RemoveBorders via reflection to stay compatible across versions
                var removeBordersProp = typeof(CadRasterizationOptions).GetProperty("RemoveBorders");
                if (removeBordersProp != null && removeBordersProp.CanWrite)
                {
                    removeBordersProp.SetValue(rasterOptions, true);
                }

                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Exported image saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}