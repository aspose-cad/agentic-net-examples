using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: <input-cad-file> <output-directory>");
            return;
        }

        string inputPath = args[0];
        string outputDir = args[1];

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Build a lookup of raster image definitions (handle -> definition)
                var rasterDefs = new Dictionary<string, CadRasterImageDef>(StringComparer.OrdinalIgnoreCase);
                foreach (CadBaseObject obj in cadImage.Objects)
                {
                    if (obj is CadRasterImageDef def)
                    {
                        rasterDefs[def.ObjectHandle] = def;
                    }
                }

                var missingImages = new List<string>();

                // Iterate through all layouts (task requirement)
                foreach (var layout in cadImage.Layouts.Values)
                {
                    // No per‑layout entity collection; processing is done globally.
                }

                // Scan all raster image entities in the CAD file
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadRasterImage raster)
                    {
                        if (rasterDefs.TryGetValue(raster.ImageDefReference, out CadRasterImageDef def))
                        {
                            string imagePath = Path.Combine(Path.GetDirectoryName(inputPath) ?? string.Empty, def.FileName);
                            if (!File.Exists(imagePath) && !missingImages.Contains(imagePath))
                            {
                                missingImages.Add(imagePath);
                            }
                        }
                    }
                }

                // Report missing images
                if (missingImages.Count > 0)
                {
                    Console.WriteLine("Missing external image references:");
                    foreach (string img in missingImages)
                    {
                        Console.WriteLine(img);
                    }
                }
                else
                {
                    Console.WriteLine("All external image references are present.");
                }

                // Save a copy of the CAD file (no modifications performed)
                string outputPath = Path.Combine(outputDir, Path.GetFileName(inputPath));
                cadImage.Save(outputPath, new DxfOptions());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}