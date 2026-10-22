using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace AddBackgroundImageToDWG
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input and output file paths
                string inputPath = "input.dwg";
                string outputPath = "output.dwg";

                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine("Input file not found: " + inputPath);
                    return;
                }

                // Load the DWG file
                using (Image loadedImage = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)loadedImage;

                    // ----- Create raster image definition (placeholder) -----
                    var rasterDef = new CadRasterImageDef
                    {
                        FileName = "background.png", // Placeholder file name
                        ImageSizeU = 1000,            // Example size
                        ImageSizeV = 800,
                        ObjectHandle = "5F"           // Unique handle
                    };

                    // Preserve existing objects and add the new definition
                    var objectsList = cadImage.Objects?.ToList() ?? new List<CadBaseObject>();
                    objectsList.Add(rasterDef);
                    cadImage.Objects = objectsList.ToArray();

                    // ----- Create raster image entity that uses the definition -----
                    var rasterImage = new CadRasterImage
                    {
                        ImageDefReference = rasterDef.ObjectHandle,
                        InsertionPoint = new Cad3DPoint(0, 0, 0),
                        UVector = new Cad3DPoint(rasterDef.ImageSizeU, 0, 0),
                        VVector = new Cad3DPoint(0, rasterDef.ImageSizeV, 0),
                        ImageSizeU = rasterDef.ImageSizeU,
                        ImageSizeV = rasterDef.ImageSizeV
                    };

                    // ----- Create a new block to hold the raster image -----
                    const string blockName = "BackgroundImageBlock";
                    var block = new CadBlockEntity();
                    block.AddEntity(rasterImage);
                    cadImage.BlockEntities.Add(blockName, block);

                    // ----- Insert the block into the drawing -----
                    var backgroundInsert = new CadInsertObject
                    {
                        Name = blockName,
                        InsertionPoint = new Cad3DPoint(0, 0, 0),
                        ScaleX = 1.0,
                        ScaleY = 1.0,
                        ScaleZ = 1.0,
                        RotationAngle = 0.0
                    };

                    // Append the insert object to the top‑level entities
                    cadImage.Entities = cadImage.Entities
                        .Concat(new CadEntityBase[] { backgroundInsert })
                        .ToArray();

                    // ----- Verify that the block contains the raster image -----
                    if (cadImage.BlockEntities.TryGetValue(blockName, out var retrievedBlock) &&
                        retrievedBlock.Entities.Any(e => e is CadRasterImage))
                    {
                        Console.WriteLine("Background image successfully added to block.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to verify background image in block.");
                    }

                    // Save the modified drawing (DWG format inferred from extension)
                    cadImage.Save(outputPath);
                }

                Console.WriteLine("File saved to: " + outputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}