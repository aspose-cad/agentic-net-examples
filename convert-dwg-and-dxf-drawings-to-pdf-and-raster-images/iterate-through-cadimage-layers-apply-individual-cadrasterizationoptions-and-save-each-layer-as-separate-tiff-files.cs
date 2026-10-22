using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
                string inputPath = "input.dwg"; // TODO: replace with actual path
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                using (Image cadImage = Image.Load(inputPath))
                {
                    CadImage cad = cadImage as CadImage;
                    if (cad == null)
                    {
                        Console.WriteLine("Failed to load CAD image.");
                        return;
                    }

                    var layerNames = cad.Entities
                        .Where(e => !string.IsNullOrEmpty(e.LayerName))
                        .Select(e => e.LayerName)
                        .Distinct()
                        .ToList();

                    int layerIndex = 0;
                    string outputDir = "output";
                    Directory.CreateDirectory(outputDir);

                    foreach (var layerName in layerNames)
                    {
                        var rasterOptions = new CadRasterizationOptions
                        {
                            PageWidth = 1600,
                            PageHeight = 1200,
                            Layers = new[] { layerName }
                        };

                        var tiffOptions = new TiffOptions
                        {
                            VectorRasterizationOptions = rasterOptions
                        };

                        string safeLayerName = string.Concat(layerName.Split(Path.GetInvalidFileNameChars()));
                        string outputPath = Path.Combine(outputDir, $"layer_{layerIndex}_{safeLayerName}.tiff");

                        cad.Save(outputPath, tiffOptions);
                        Console.WriteLine($"Saved layer '{layerName}' to {outputPath}");
                        layerIndex++;
                    }
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}