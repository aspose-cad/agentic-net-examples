using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the DWG/DXF drawing
            using (Image image = Image.Load(inputPath))
            {
                var cadImage = (CadImage)image;

                // Build a set of existing layer names for quick lookup
                var existingLayers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    if (layer != null && !string.IsNullOrEmpty(layer.Name))
                        existingLayers.Add(layer.Name);
                }

                // Iterate over all block definitions in the drawing
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    // Examine each entity inside the block definition
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        // Look for block insert references
                        if (entity is CadInsertObject insert)
                        {
                            // Find the block definition that the insert references
                            CadBlockEntity referencedBlock = FindBlockByName(cadImage, insert.Name);
                            if (referencedBlock == null)
                                continue;

                            // Ensure every layer used by entities inside the referenced block exists in the drawing
                            foreach (CadEntityBase innerEntity in referencedBlock.Entities)
                            {
                                string layerName = innerEntity.LayerName;
                                if (string.IsNullOrEmpty(layerName))
                                    continue;

                                if (!existingLayers.Contains(layerName))
                                {
                                    // Add missing layer to the drawing's layer collection
                                    var newLayer = new CadLayerTable { Name = layerName };
                                    cadImage.Layers.Add(newLayer);
                                    existingLayers.Add(layerName);
                                    Console.WriteLine($"Added missing layer: {layerName}");
                                }
                            }
                        }
                    }
                }

                // Save the modified drawing
                cadImage.Save(outputPath);
                Console.WriteLine($"Saved modified drawing to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }

    // Helper to locate a block definition by its name
    private static CadBlockEntity FindBlockByName(CadImage cadImage, string blockName)
    {
        foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
        {
            if (block.Name.Equals(blockName, StringComparison.OrdinalIgnoreCase))
                return block;
        }
        return null;
    }
}