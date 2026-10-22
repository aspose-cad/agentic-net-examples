using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        // Placeholder paths – replace with actual file locations.
        string sourcePath = "input.dwg";
        string outputFolder = "output";

        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        Directory.CreateDirectory(outputFolder);

        try
        {
            using (Image cadImage = Image.Load(sourcePath))
            {
                CadImage cad = (CadImage)cadImage;

                // cad.Layers returns a CadLayersList; iterate over its items (CadLayerTable)
                var layerList = cad.Layers;

                foreach (CadLayerTable layerRecord in layerList)
                {
                    string layerName = layerRecord.Name;

                    // Collect entities that belong to the current layer
                    CadEntityBase[] layerEntities = cad.Entities
                        .Where(e => string.Equals(e.LayerName, layerName, StringComparison.OrdinalIgnoreCase))
                        .ToArray();

                    // Skip if the layer has no entities
                    if (layerEntities.Length == 0)
                        continue;

                    // Configure rasterization options for the specific layer
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200,
                        Layouts = new[] { "Model" },
                        Layers = new[] { layerName }
                    };

                    // Set up PNG export options
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    string outputPath = Path.Combine(outputFolder, $"{layerName}.png");

                    // Export the layer to PNG
                    cadImage.Save(outputPath, pngOptions);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}