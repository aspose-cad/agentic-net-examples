using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Expect input and output paths as arguments
        if (args.Length < 2)
        {
            Console.Error.WriteLine("Usage: program <input.cad> <output.pdf>");
            return;
        }

        string inputPath = args[0];
        string outputPath = args[1];

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file does not exist: {inputPath}");
            return;
        }

        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            try
            {
                Directory.CreateDirectory(outputDir);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }
        }

        CadImage cadImage = null;
        try
        {
            // Load CAD file
            using (Aspose.CAD.Image image = Aspose.CAD.Image.Load(inputPath))
            {
                cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Define criteria for removal
                string layerToRemove = "HiddenLayer";

                // Remove entities from top‑level collection (if mutable)
                RemoveEntitiesFromCollection(cadImage.Entities, layerToRemove);

                // Remove entities from each block definition
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    RemoveEntitiesFromCollection(block.Entities, layerToRemove);
                }

                // After removal, check if any entities remain
                bool anyTopLevel = cadImage.Entities.Any();
                bool anyBlock = cadImage.BlockEntities.ValuesTyped.Any(b => b.Entities.Any());

                if (!anyTopLevel && !anyBlock)
                {
                    Console.Error.WriteLine("All entities were removed; nothing to export.");
                    return;
                }

                // Prepare rasterization options for PDF export
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new[] { "Model" }
                };

                // Export only visible layers (exclude the removed layer)
                List<string> visibleLayers = new List<string>();
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    // Skip the layer we removed
                    if (string.Equals(layer.Name, layerToRemove, StringComparison.OrdinalIgnoreCase))
                        continue;

                    // Check layer visibility flags
                    bool isOff = layer.ColorId < 0;
                    bool isFrozen = (layer.Flags & 1) != 0;
                    // Locked layers are still visible for export
                    if (!isOff && !isFrozen)
                    {
                        visibleLayers.Add(layer.Name);
                    }
                }
                rasterOptions.Layers = visibleLayers.ToArray();

                // Set PDF options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Export to PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Exported filtered CAD to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
        }
        finally
        {
            cadImage?.Dispose();
        }
    }

    // Removes entities matching criteria from a collection if the collection supports IList<T>
    private static void RemoveEntitiesFromCollection(IEnumerable<CadEntityBase> entitiesEnum, string layerToRemove)
    {
        if (entitiesEnum is IList<CadEntityBase> entities)
        {
            List<int> indicesToRemove = new List<int>();
            for (int i = 0; i < entities.Count; i++)
            {
                CadEntityBase entity = entities[i];

                // Remove by layer name
                if (string.Equals(entity.LayerName, layerToRemove, StringComparison.OrdinalIgnoreCase))
                {
                    indicesToRemove.Add(i);
                    continue;
                }

                // Remove circles
                if (entity is CadCircle)
                {
                    indicesToRemove.Add(i);
                    continue;
                }

                // Remove arcs with zero length
                if (entity is CadArc arc)
                {
                    if (Math.Abs(arc.StartAngle - arc.EndAngle) < 1e-6)
                    {
                        indicesToRemove.Add(i);
                    }
                }
            }

            // Remove in reverse order to keep indices valid
            for (int i = indicesToRemove.Count - 1; i >= 0; i--)
            {
                entities.RemoveAt(indicesToRemove[i]);
            }
        }
        // If the collection is not mutable, we simply skip removal.
    }
}