using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadLayerPolylineMapping
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the CAD drawing (DWG or DXF). Replace with actual file path or pass as argument.
                string cadFilePath = args.Length > 0 ? args[0] : "sample.dwg";

                // Guard against missing file.
                if (!File.Exists(cadFilePath))
                {
                    Console.Error.WriteLine($"File not found: {cadFilePath}");
                    return;
                }

                // Load the CAD drawing.
                using (Image image = Image.Load(cadFilePath))
                {
                    // Cast the loaded image to CadImage to access CAD‑specific members.
                    CadImage cadImage = (CadImage)image;

                    // Prepare a dictionary to map layer names to their polyline entities.
                    var layerToPolylines = new Dictionary<string, List<CadLwPolyline>>(StringComparer.OrdinalIgnoreCase);

                    // Initialize dictionary entries for all layers present in the drawing.
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        string layerName = layer.Name ?? string.Empty;
                        if (!layerToPolylines.ContainsKey(layerName))
                        {
                            layerToPolylines[layerName] = new List<CadLwPolyline>();
                        }
                    }

                    // Iterate over top‑level drawing entities.
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Check if the entity is a lightweight polyline.
                        if (entity is CadLwPolyline polyline)
                        {
                            // Retrieve the layer name of the polyline.
                            string polylineLayer = polyline.LayerName ?? string.Empty;

                            // Ensure the layer exists in the dictionary.
                            if (!layerToPolylines.ContainsKey(polylineLayer))
                            {
                                layerToPolylines[polylineLayer] = new List<CadLwPolyline>();
                            }

                            // Add the polyline to the corresponding layer list.
                            layerToPolylines[polylineLayer].Add(polyline);
                        }
                    }

                    // Output the mapping results.
                    foreach (KeyValuePair<string, List<CadLwPolyline>> kvp in layerToPolylines)
                    {
                        string layerName = kvp.Key;
                        List<CadLwPolyline> polylines = kvp.Value;

                        Console.WriteLine($"Layer: {layerName} - Polyline count: {polylines.Count}");
                        foreach (CadLwPolyline pl in polylines)
                        {
                            int pointCount = pl.Coordinates?.Count ?? 0;
                            Console.WriteLine($"  Polyline with {pointCount} points.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}