using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            string dxfPath = "input.dxf";
            string backgroundImagePath = "background.png";
            string outputPath = "output.dxf";

            if (!File.Exists(dxfPath))
            {
                Console.WriteLine($"DXF file not found: {dxfPath}");
                return;
            }

            if (!File.Exists(backgroundImagePath))
            {
                Console.WriteLine($"Background image file not found: {backgroundImagePath}");
                return;
            }

            using (Image image = Image.Load(dxfPath))
            {
                CadImage cadImage = (CadImage)image;

                // Load background image bytes (not directly used, but kept for completeness)
                byte[] bgBytes = File.ReadAllBytes(backgroundImagePath);

                // Create raster image definition
                var rasterDef = new CadRasterImageDef
                {
                    FileName = backgroundImagePath,
                    ImageSizeU = 100, // placeholder size
                    ImageSizeV = 100, // placeholder size
                    ObjectHandle = $"DEF{Guid.NewGuid():N}"
                };

                // Add definition to the drawing objects
                var existingObjects = cadImage.Objects ?? new CadBaseObject[0];
                cadImage.Objects = existingObjects.Concat(new CadBaseObject[] { rasterDef }).ToArray();

                // Create raster image entity
                var insertionPoint = new Cad3DPoint(0, 0, 0);
                var uVector = new Cad3DPoint(rasterDef.ImageSizeU, 0, 0);
                var vVector = new Cad3DPoint(0, rasterDef.ImageSizeV, 0);
                var rasterImage = new CadRasterImage(rasterDef, insertionPoint, uVector, vVector)
                {
                    ImageDefReference = rasterDef.ObjectHandle,
                    ImageSizeU = rasterDef.ImageSizeU,
                    ImageSizeV = rasterDef.ImageSizeV
                };

                // Add raster image to entities
                var existingEntities = cadImage.Entities ?? new CadEntityBase[0];
                cadImage.Entities = existingEntities.Concat(new CadEntityBase[] { rasterImage }).ToArray();

                // Collect new TEXT entities
                var newTexts = new List<CadEntityBase>();
                foreach (var entity in cadImage.Entities)
                {
                    if (entity is CadText existingText)
                    {
                        var oldPt = existingText.FirstAlignment;
                        var newPt = new Cad3DPoint(oldPt.X + 10, oldPt.Y, oldPt.Z);

                        var copyText = new CadText
                        {
                            DefaultValue = existingText.DefaultValue + " - copy",
                            FirstAlignment = newPt,
                            TextHeight = existingText.TextHeight,
                            TextRotation = existingText.TextRotation,
                            SoftOwner = existingText.SoftOwner
                        };

                        newTexts.Add(copyText);
                    }
                }

                // Append new TEXT entities
                cadImage.Entities = cadImage.Entities.Concat(newTexts).ToArray();

                // Save the modified DXF
                cadImage.Save(outputPath, new DxfOptions());
                Console.WriteLine($"Modified DXF saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}