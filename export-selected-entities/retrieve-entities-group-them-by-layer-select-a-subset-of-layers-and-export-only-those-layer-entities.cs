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
            // Input CAD file path
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Output PNG file path
            string outputPath = "output.png";

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cadImage = (CadImage)image;

                // Retrieve all top‑level entities
                IEnumerable<CadEntityBase> allEntities = cadImage.Entities;

                // Group entities by their layer name
                IEnumerable<IGrouping<string, CadEntityBase>> entitiesByLayer = allEntities.GroupBy(e => e.LayerName);

                // Example: select a subset of layers to export
                string[] selectedLayers = new string[] { "Layer1", "Layer2" };

                // Optional: display entity counts per layer
                foreach (IGrouping<string, CadEntityBase> layerGroup in entitiesByLayer)
                {
                    Console.WriteLine($"Layer: {layerGroup.Key}, Entity Count: {layerGroup.Count()}");
                }

                // Configure rasterization options to include only the selected layers
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layers = selectedLayers,
                    PageWidth = 1600,
                    PageHeight = 1200
                };

                // Set up PNG save options with the rasterization settings
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the drawing; only entities from the selected layers will be rendered
                cadImage.Save(outputPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}