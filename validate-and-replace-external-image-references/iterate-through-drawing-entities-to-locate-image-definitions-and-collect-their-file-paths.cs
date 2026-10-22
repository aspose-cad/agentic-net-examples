using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = args.Length > 0 ? args[0] : "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            List<string> imagePaths = new List<string>();

            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("The file is not a CAD image.");
                    return;
                }

                // Map raster image definition handles to their file names
                var rasterDefMap = cadImage.Objects
                    .OfType<CadRasterImageDef>()
                    .ToDictionary(def => def.ObjectHandle, def => def.FileName);

                // Top‑level entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    CollectImagePath(entity, imagePaths, rasterDefMap);
                }

                // Block definitions
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        CollectImagePath(entity, imagePaths, rasterDefMap);
                    }
                }
            }

            foreach (string path in imagePaths)
            {
                Console.WriteLine(path);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }

    static void CollectImagePath(CadEntityBase entity, List<string> imagePaths, Dictionary<string, string> rasterDefMap)
    {
        if (entity is CadRasterImage rasterImage)
        {
            string handle = rasterImage.ImageDefReference;
            if (!string.IsNullOrEmpty(handle) && rasterDefMap.TryGetValue(handle, out string fileName))
            {
                if (!string.IsNullOrEmpty(fileName) && !imagePaths.Contains(fileName))
                {
                    imagePaths.Add(fileName);
                }
            }
        }
        // Block entities are processed separately in Main.
    }
}