using System;
using System.IO;
using System.Collections.Generic;
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
            if (args == null || args.Length < 3)
            {
                Console.Error.WriteLine("Usage: <program> <input.dwg> <output.dwg> <placeholder.png>");
                return;
            }

            string inputPath = args[0];
            string outputPath = args[1];
            string placeholderPath = args[2];

            if (string.IsNullOrEmpty(inputPath) || !File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input CAD file not found: {inputPath}");
                return;
            }

            if (string.IsNullOrEmpty(placeholderPath) || !File.Exists(placeholderPath))
            {
                Console.Error.WriteLine($"Placeholder image file not found: {placeholderPath}");
                return;
            }

            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            using (Image rawImage = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)rawImage;

                // Build a lookup for raster image definitions.
                var rasterDefLookup = BuildRasterDefLookup(cadImage);

                // Process top‑level entities.
                ProcessEntityCollection(cadImage.Entities, cadImage, rasterDefLookup, placeholderPath);

                // Process block entities.
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    ProcessEntityCollection(block.Entities, cadImage, rasterDefLookup, placeholderPath);
                }

                // Save the modified drawing.
                cadImage.Save(outputPath);
            }

            Console.WriteLine("Processing completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    private static Dictionary<string, CadRasterImageDef> BuildRasterDefLookup(CadImage cadImage)
    {
        var dict = new Dictionary<string, CadRasterImageDef>(StringComparer.OrdinalIgnoreCase);
        foreach (var obj in cadImage.Objects)
        {
            if (obj is CadRasterImageDef rasterDef && !string.IsNullOrEmpty(rasterDef.ObjectHandle))
            {
                dict[rasterDef.ObjectHandle] = rasterDef;
            }
        }
        return dict;
    }

    private static void ProcessEntityCollection(IEnumerable<CadEntityBase> entities,
                                                 CadImage cadImage,
                                                 Dictionary<string, CadRasterImageDef> rasterDefLookup,
                                                 string placeholderPath)
    {
        foreach (var entity in entities)
        {
            if (entity is CadRasterImage rasterImage)
            {
                string defRef = rasterImage.ImageDefReference;
                CadRasterImageDef rasterDef = null;

                if (!string.IsNullOrEmpty(defRef) && rasterDefLookup.TryGetValue(defRef, out rasterDef))
                {
                    Console.WriteLine($"Raster image definition file: {rasterDef.FileName}");
                }
                else
                {
                    Console.WriteLine("Raster image definition not found for entity.");
                }

                // Determine if the referenced file exists.
                string currentFilePath = rasterDef?.FileName;
                bool fileMissing = string.IsNullOrEmpty(currentFilePath) || !File.Exists(currentFilePath);

                if (fileMissing)
                {
                    // Load placeholder to obtain dimensions.
                    using (Image placeholderImg = Image.Load(placeholderPath))
                    {
                        int width = placeholderImg.Width;
                        int height = placeholderImg.Height;

                        // Create a new raster definition for the placeholder.
                        var placeholderDef = new CadRasterImageDef(placeholderPath, width, height);
                        string newHandle = "H" + Guid.NewGuid().ToString("N").Substring(0, 8);
                        placeholderDef.ObjectHandle = newHandle;

                        // Add the new definition to the drawing objects.
                        var objectsList = cadImage.Objects.ToList();
                        objectsList.Add(placeholderDef);
                        cadImage.Objects = objectsList.ToArray();

                        // Update lookup.
                        rasterDefLookup[newHandle] = placeholderDef;

                        // Point the raster image entity to the new definition.
                        rasterImage.ImageDefReference = newHandle;

                        Console.WriteLine($"Replaced missing image with placeholder: {placeholderPath}");
                    }
                }
            }
        }
    }
}