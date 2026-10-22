using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadConsts;

namespace InsertExternalDwgs
{
    class Program
    {
        static void Main(string[] args)
        {
            string mainDrawingPath = @"C:\CAD\MainDrawing.dwg";
            string externalDrawingPath = @"C:\CAD\ExternalDrawing.dwg";
            string outputPath = @"C:\CAD\CombinedDrawing.dwg";

            if (!File.Exists(mainDrawingPath))
            {
                Console.WriteLine($"Main drawing not found: {mainDrawingPath}");
                return;
            }

            if (!File.Exists(externalDrawingPath))
            {
                Console.WriteLine($"External drawing not found: {externalDrawingPath}");
                return;
            }

            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                using (Image mainImg = Image.Load(mainDrawingPath))
                {
                    CadImage mainCad = mainImg as CadImage;
                    if (mainCad == null)
                    {
                        Console.WriteLine("Failed to cast main image to CadImage.");
                        return;
                    }

                    using (Image extImg = Image.Load(externalDrawingPath))
                    {
                        CadImage extCad = extImg as CadImage;
                        if (extCad == null)
                        {
                            Console.WriteLine("Failed to cast external image to CadImage.");
                            return;
                        }

                        // Create a new block entity to hold external drawing entities
                        CadBlockEntity externalBlock = new CadBlockEntity();
                        string blockKey = "ExternalBlock";
                        externalBlock.Name = blockKey;
                        externalBlock.BlockHandle = "BLOCK_EXTERNAL";

                        // Add each entity from the external drawing into the block
                        foreach (var entity in extCad.Entities)
                        {
                            externalBlock.AddEntity(entity);
                        }

                        // Add the block to the main drawing if it does not already exist
                        if (!mainCad.BlockEntities.ContainsKey(blockKey))
                        {
                            mainCad.BlockEntities.Add(blockKey, externalBlock);
                        }
                        else
                        {
                            Console.WriteLine($"Block key '{blockKey}' already exists in the main drawing.");
                        }

                        // Save the combined drawing
                        DxfOptions saveOptions = new DxfOptions
                        {
                            OutputMode = CadOutputMode.Convert,
                            DxfFileFormat = CadFileFormat.Binary
                        };
                        mainCad.Save(outputPath, saveOptions);
                        Console.WriteLine($"Combined drawing saved to: {outputPath}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}