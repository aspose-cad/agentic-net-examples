using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load("input.dwg"))
            {
                CadImage cadImage = (CadImage)image;

                // Group entities by their layer name
                var layerGroups = new Dictionary<string, List<CadEntityBase>>(StringComparer.OrdinalIgnoreCase);
                foreach (var entity in cadImage.Entities)
                {
                    string layerName = entity.LayerName ?? "0";
                    if (!layerGroups.ContainsKey(layerName))
                        layerGroups[layerName] = new List<CadEntityBase>();
                    layerGroups[layerName].Add(entity);
                }

                // Ensure output directory exists
                string outputDir = "output";
                if (!Directory.Exists(outputDir))
                    Directory.CreateDirectory(outputDir);

                // Export each layer as a separate PNG file
                foreach (var kvp in layerGroups)
                {
                    string layerName = kvp.Key;
                    CadEntityBase[] layerEntities = kvp.Value.ToArray(); // array for this layer (as requested)

                    // Configure rasterization to render only the current layer
                    var rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200,
                        Layouts = new[] { "Model" },
                        Layers = new[] { layerName }
                    };

                    var pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Build a safe file name
                    string safeLayerName = string.Concat(layerName.Split(Path.GetInvalidFileNameChars()));
                    string outputFile = Path.Combine(outputDir, $"output_{safeLayerName}.png");

                    cadImage.Save(outputFile, pngOptions);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}