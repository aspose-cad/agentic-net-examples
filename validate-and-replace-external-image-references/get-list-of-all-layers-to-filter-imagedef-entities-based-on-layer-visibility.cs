using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "input.dxf";
        string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD file
            CadImage cadImage = (CadImage)Image.Load(inputPath);

            // Determine visible layers based on ColorId and Flags
            HashSet<string> visibleLayers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (CadLayerTable layer in cadImage.Layers)
            {
                bool isOff = layer.ColorId < 0;                 // off = negative color index
                bool isFrozen = (layer.Flags & 1) != 0;        // frozen = bit 0 set
                if (!isOff && !isFrozen)
                {
                    visibleLayers.Add(layer.Name);
                }
            }

            // Filter raster image entities (IMAGE) that reside on visible layers
            var visibleRasterImages = cadImage.Entities
                .OfType<CadRasterImage>()
                .Where(img => visibleLayers.Contains(img.LayerName))
                .ToArray();

            // Output information about the visible raster images
            Console.WriteLine("Visible IMAGE entities:");
            foreach (CadRasterImage raster in visibleRasterImages)
            {
                // Resolve the raster definition to obtain the file name
                string defHandle = raster.ImageDefReference;
                CadRasterImageDef rasterDef = cadImage.Objects
                    .OfType<CadRasterImageDef>()
                    .FirstOrDefault(d => d.ObjectHandle == defHandle);

                string fileName = rasterDef != null ? rasterDef.FileName : "Unknown";
                Console.WriteLine($"- Layer: {raster.LayerName}, File: {fileName}");
            }

            // Replace the entity collection with only visible entities
            var filteredEntities = cadImage.Entities
                .Where(e =>
                {
                    if (e.TypeName == CadEntityTypeName.IMAGE)
                    {
                        var img = e as CadRasterImage;
                        return img != null && visibleLayers.Contains(img.LayerName);
                    }
                    return true; // keep non-image entities
                })
                .ToArray();

            cadImage.Entities = filteredEntities;

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Save the CAD file preserving only visible entities
            var dxfOptions = new DxfOptions
            {
                OutputMode = CadOutputMode.Convert,
                DxfFileFormat = CadFileFormat.Ascii
            };
            cadImage.Save(outputPath, dxfOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}