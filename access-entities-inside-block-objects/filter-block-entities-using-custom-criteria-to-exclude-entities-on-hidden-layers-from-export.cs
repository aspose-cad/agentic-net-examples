using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.ImageOptions;

class Program
{
    static int Main(string[] args)
    {
        string inputPath = args.Length > 0 ? args[0] : "input.dxf";
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return 0;
        }
        try
        {
            using Image image = Image.Load(inputPath);
            if (image is not CadImage cadImage)
                throw new NotSupportedException("A DWG or DXF drawing is required.");

            var hiddenLayers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (CadLayerTable layer in cadImage.Layers)
            {
                // Off: negative color number. Frozen: bit 1. Locked (bit 4)
                // and default freezing in new viewports (bit 2) do not hide a
                // layer globally. PlotFlag is a separate plotting setting.
                if (layer.ColorId < 0 || (layer.Flags & 1) != 0)
                {
                    hiddenLayers.Add(layer.Name);
                    Console.WriteLine($"Excluded layer: {layer.Name}");
                }
            }

            foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
            {
                var retained = new List<CadEntityBase>();
                int removed = 0;
                foreach (CadEntityBase entity in block.Entities)
                {
                    string layer = string.IsNullOrEmpty(entity.LayerName) ? "0" : entity.LayerName;
                    // Layer 0 inside a block inherits the insertion's layer.
                    if (layer != "0" && hiddenLayers.Contains(layer)) { removed++; continue; }
                    retained.Add(entity);
                    Console.WriteLine($"Retained block entity: {entity.GetType().Name} on {layer}");
                }
                block.Entities = retained;
                Console.WriteLine($"Block '{block.Name}': kept {retained.Count}, excluded {removed}");
            }

            var drawingEntities = new List<CadEntityBase>();
            foreach (CadEntityBase entity in cadImage.Entities)
            {
                string layer = string.IsNullOrEmpty(entity.LayerName) ? "0" : entity.LayerName;
                if (!hiddenLayers.Contains(layer)) drawingEntities.Add(entity);
            }
            cadImage.Entities = drawingEntities.ToArray();
            bool hasGeometry = false;
            foreach (CadEntityBase entity in drawingEntities)
                hasGeometry |= HasGeometry(cadImage, entity, new HashSet<string>(StringComparer.OrdinalIgnoreCase));
            if (!hasGeometry)
            {
                Console.WriteLine("No visible entities remain; no output was written.");
                return 0;
            }

            string outputDir = "output";
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, "visible_block_entities.png");
            var options = new PngOptions
            {
                VectorRasterizationOptions = new CadRasterizationOptions { PageWidth = 800, PageHeight = 600 }
            };
            cadImage.Save(outputPath, options);
            Console.WriteLine($"Exported visible geometry to {outputPath}");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return 1;
        }
    }

    static bool HasGeometry(CadImage cad, CadEntityBase entity, HashSet<string> active)
    {
        if (entity is not CadInsertObject insert) return true;
        string name = insert.Name ?? insert.OriginalBlockName;
        if (string.IsNullOrEmpty(name) || !cad.BlockEntities.TryGetValue(name, out CadBlockEntity block))
            throw new InvalidOperationException("Referenced block definition was not found.");
        if (!active.Add(name)) throw new InvalidOperationException($"Cyclic block reference: {name}");
        try
        {
            foreach (CadEntityBase child in block.Entities)
                if (HasGeometry(cad, child, active)) return true;
            return false;
        }
        finally { active.Remove(name); }
    }
}