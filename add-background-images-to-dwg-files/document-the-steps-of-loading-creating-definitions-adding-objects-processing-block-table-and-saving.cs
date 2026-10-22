using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            string sourcePath = "input.dwg";
            string backgroundImagePath = "background.png";
            string outputPath = "output.dwg";

            if (!File.Exists(sourcePath))
            {
                Console.WriteLine($"Source DWG file not found: {sourcePath}");
                return;
            }

            if (!File.Exists(backgroundImagePath))
            {
                Console.WriteLine($"Background image file not found: {backgroundImagePath}");
                return;
            }

            using (Image loadedImage = Image.Load(sourcePath))
            {
                CadImage cadImage = loadedImage as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                const string blockName = "BackgroundBlock";

                // Ensure block exists
                if (!cadImage.BlockEntities.ContainsKey(blockName))
                {
                    var blockEntity = new CadBlockEntity
                    {
                        BasePoint = new Cad3DPoint(0, 0, 0)
                    };
                    cadImage.BlockEntities.Add(blockName, blockEntity);
                }

                // Create raster image definition (placeholder size values)
                var rasterDef = new CadRasterImageDef(backgroundImagePath, 1000, 1000);
                // Add definition to Objects collection
                cadImage.Objects = cadImage.Objects.Concat(new CadBaseObject[] { rasterDef }).ToArray();

                // Create raster image entity referencing the definition
                var rasterImage = new CadRasterImage(
                    rasterDef,
                    new Cad3DPoint(0, 0, 0),          // Insertion point
                    new Cad3DPoint(1, 0, 0),          // U vector
                    new Cad3DPoint(0, 1, 0)           // V vector
                );

                // Add raster image to the block's entities
                var block = cadImage.BlockEntities[blockName];
                block.Entities = block.Entities.Concat(new CadEntityBase[] { rasterImage }).ToArray();

                // Insert the block into the drawing
                var insertObject = new CadInsertObject
                {
                    InsertionPoint = new Cad3DPoint(0, 0, 0),
                    Name = blockName,
                    ScaleX = 1.0,
                    ScaleY = 1.0,
                    ScaleZ = 1.0,
                    RotationAngle = 0.0
                };

                // Add the insert object to top‑level entities
                cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { insertObject }).ToArray();

                // Save the modified DWG
                cadImage.Save(outputPath);
            }

            Console.WriteLine("Background image block added and DWG saved successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}