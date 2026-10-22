using Aspose.CAD;
using System;
using System.IO;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";
            string outputPath = "attributes.txt";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outDir) && !Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }

            using (var cadImage = Aspose.CAD.Image.Load(inputPath) as CadImage)
            {
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to load CAD image.");
                    return;
                }

                using (var writer = new StreamWriter(outputPath, false))
                {
                    // Iterate top‑level insert objects
                    foreach (var entity in cadImage.Entities)
                    {
                        if (entity is CadInsertObject insert)
                        {
                            string blockName = insert.Name ?? insert.OriginalBlockName;
                            writer.WriteLine($"Insert Block: {blockName}");

                            // Locate the block definition
                            CadBlockEntity blockDef = null;
                            foreach (var blk in cadImage.BlockEntities.ValuesTyped)
                            {
                                // Block name property is not part of core rules; using free‑form access
                                var blkNameProp = blk.GetType().GetProperty("Name");
                                if (blkNameProp != null && blkNameProp.GetValue(blk) as string == blockName)
                                {
                                    blockDef = blk;
                                    break;
                                }
                            }

                            if (blockDef == null)
                            {
                                writer.WriteLine("  Block definition not found.");
                                continue;
                            }

                            // Search for attribute entities inside the block
                            foreach (var blkEntity in blockDef.Entities)
                            {
                                string typeName = blkEntity.GetType().Name;
                                if (typeName.Contains("Attribute"))
                                {
                                    // Use dynamic to read common attribute members (Tag, Text)
                                    dynamic attr = blkEntity;
                                    string tag = attr.Tag ?? string.Empty;
                                    string value = attr.Text ?? string.Empty;
                                    writer.WriteLine($"  Attribute - Tag: {tag}, Value: {value}");
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"Attribute information saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}