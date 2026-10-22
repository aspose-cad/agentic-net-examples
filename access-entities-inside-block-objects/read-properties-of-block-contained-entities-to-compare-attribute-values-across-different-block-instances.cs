using Aspose.CAD;
using System;
using System.IO;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main(string[] args)
    {
        try
        {
                string inputPath = args.Length > 0 ? args[0] : "sample.dxf";

                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file '{inputPath}' does not exist.");
                    return;
                }

                // Load the CAD file
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("Failed to load CAD image.");
                        return;
                    }

                    // Collect information about each block instance
                    System.Collections.Generic.List<BlockInstanceInfo> blockInfos = new System.Collections.Generic.List<BlockInstanceInfo>();

                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        CadInsertObject insert = entity as CadInsertObject;
                        if (insert != null)
                        {
                            string blockName = insert.Name;

                            // Retrieve the block definition
                            CadBlockEntity blockDef;
                            if (cadImage.BlockEntities.TryGetValue(blockName, out blockDef))
                            {
                                BlockInstanceInfo info = new BlockInstanceInfo();
                                info.BlockName = blockName;
                                info.InsertionPointX = insert.InsertionPoint.X;
                                info.InsertionPointY = insert.InsertionPoint.Y;
                                info.InsertionPointZ = insert.InsertionPoint.Z;

                                // Example: count line entities inside the block definition
                                int lineCount = 0;
                                foreach (CadEntityBase blockEntity in blockDef.Entities)
                                {
                                    if (blockEntity is CadLine)
                                    {
                                        lineCount++;
                                    }
                                }
                                info.LineEntityCount = lineCount;

                                blockInfos.Add(info);
                            }
                        }
                    }

                    Console.WriteLine($"Comparing {blockInfos.Count} block instances.");

                    // Compare attribute values across block instances (here we compare line counts)
                    for (int i = 0; i < blockInfos.Count; i++)
                    {
                        for (int j = i + 1; j < blockInfos.Count; j++)
                        {
                            BlockInstanceInfo a = blockInfos[i];
                            BlockInstanceInfo b = blockInfos[j];

                            if (a.BlockName == b.BlockName && a.LineEntityCount != b.LineEntityCount)
                            {
                                Console.WriteLine($"Block '{a.BlockName}' instances at ({a.InsertionPointX}, {a.InsertionPointY}, {a.InsertionPointZ}) and ({b.InsertionPointX}, {b.InsertionPointY}, {b.InsertionPointZ}) have different line counts: {a.LineEntityCount} vs {b.LineEntityCount}");
                            }
                        }
                    }
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}

    // DTO to hold information about a block instance
    private class BlockInstanceInfo
    {
        public string BlockName { get; set; }
        public double InsertionPointX { get; set; }
        public double InsertionPointY { get; set; }
        public double InsertionPointZ { get; set; }
        public int LineEntityCount { get; set; }
    }
}