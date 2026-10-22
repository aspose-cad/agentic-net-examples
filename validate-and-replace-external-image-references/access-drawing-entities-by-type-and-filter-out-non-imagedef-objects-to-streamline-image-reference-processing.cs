using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        const string inputPath = "sample.dxf";
        const string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            var cadImage = (CadImage)Image.Load(inputPath);

            // Retrieve only external raster image definitions (IMAGEDEF objects)
            var rasterDefs = cadImage.Objects
                .OfType<CadRasterImageDef>()
                .ToList();

            // Process each raster definition
            foreach (var rasterDef in rasterDefs)
            {
                Console.WriteLine($"Raster definition handle: {rasterDef.ObjectHandle}");
                Console.WriteLine($"File name: {rasterDef.FileName}");
                Console.WriteLine($"Size U: {rasterDef.ImageSizeU}, Size V: {rasterDef.ImageSizeV}");
            }

            // Filter raster image entities that reference the retrieved IMAGEDEF objects
            var rasterImages = cadImage.Entities
                .OfType<CadRasterImage>()
                .Where(img => rasterDefs.Any(def => def.ObjectHandle == img.ImageDefReference))
                .ToArray();

            Console.WriteLine($"Found {rasterImages.Length} raster image entities linked to IMAGEDEF objects.");

            // Ensure output directory exists
            var outDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outDir))
                Directory.CreateDirectory(outDir);

            // Save the drawing preserving editable objects
            var dxfOptions = new DxfOptions
            {
                OutputMode = CadOutputMode.Convert,
                DxfFileFormat = CadFileFormat.Binary
            };
            cadImage.Save(outputPath, dxfOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}