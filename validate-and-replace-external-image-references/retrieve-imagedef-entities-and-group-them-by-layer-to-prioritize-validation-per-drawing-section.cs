using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            var cadImage = (CadImage)Image.Load(inputPath);

            // Build a lookup table for external raster definitions (IMAGEDEF objects)
            var rasterDefs = cadImage.Objects
                .OfType<CadRasterImageDef>()
                .ToDictionary(def => def.ObjectHandle, def => def);

            // Select raster image entities
            var rasterImages = cadImage.Entities
                .OfType<CadRasterImage>();

            // Group raster images by layer name (if available, otherwise use "Default")
            var rasterImagesByLayer = rasterImages
                .GroupBy(img =>
                {
                    var layerProp = img.GetType().GetProperty("LayerName");
                    if (layerProp != null)
                    {
                        var value = layerProp.GetValue(img) as string;
                        return string.IsNullOrEmpty(value) ? "Default" : value;
                    }
                    return "Default";
                });

            // Iterate through each layer group and display associated IMAGEDEF information
            foreach (var layerGroup in rasterImagesByLayer)
            {
                Console.WriteLine($"Layer: {layerGroup.Key}");
                foreach (var rasterImg in layerGroup)
                {
                    // Resolve the IMAGEDEF definition referenced by the raster image
                    if (rasterDefs.TryGetValue(rasterImg.ImageDefReference, out CadRasterImageDef def))
                    {
                        Console.WriteLine($"  IMAGEDEF Handle: {def.ObjectHandle}, FileName: {def.FileName}");
                    }
                    else
                    {
                        Console.WriteLine($"  IMAGEDEF not found for reference: {rasterImg.ImageDefReference}");
                    }
                }
            }

            // Example of saving the drawing while preserving editable objects (optional)
            // string outputPath = "output.dxf";
            // Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            // cadImage.Save(outputPath, new DxfOptions
            // {
            //     OutputMode = CadOutputMode.Convert,
            //     DxfFileFormat = CadConsts.CadFileFormat.Binary
            // });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}