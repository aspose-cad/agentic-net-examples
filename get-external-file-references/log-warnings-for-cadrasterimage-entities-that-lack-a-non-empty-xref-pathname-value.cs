using System;
using System.Collections.Generic;
using System.IO;
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
            string inputPath = args.Length > 0 ? args[0] : "sample.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to load CAD image.");
                    return;
                }

                // Inspect block definitions
                CadBlockDictionary blockDict = cadImage.BlockEntities;
                foreach (KeyValuePair<string, CadBlockEntity> kvp in blockDict)
                {
                    string blockName = kvp.Key;
                    CadBlockEntity block = kvp.Value;
                    Console.WriteLine($"Block: {blockName}");

                    var blockEntities = block.Entities; // ICollection<CadEntityBase>
                    Console.WriteLine($"  Contains {blockEntities.Count} entities.");
                    foreach (CadEntityBase entity in blockEntities)
                    {
                        Console.WriteLine($"    Entity Type: {entity.TypeName}");
                    }
                }

                // Log warnings for raster images lacking a non‑empty XRef PathName
                IEnumerable<CadEntityBase> topEntities = cadImage.Entities;
                foreach (CadEntityBase entity in topEntities)
                {
                    if (entity is CadRasterImage raster)
                    {
                        string xref = raster.ImageDefReference;
                        if (string.IsNullOrEmpty(xref))
                        {
                            Console.WriteLine($"Warning: CadRasterImage entity at insertion point {raster.InsertionPoint} lacks a non‑empty XRef PathName.");
                        }
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