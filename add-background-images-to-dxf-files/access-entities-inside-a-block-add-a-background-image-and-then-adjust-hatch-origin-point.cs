using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dxf";
        const string backgroundPath = "background.png";
        const string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        if (!File.Exists(backgroundPath))
        {
            Console.Error.WriteLine($"Background image not found: {backgroundPath}");
            return;
        }

        try
        {
            // Load the DXF file as a CadImage
            using (Image img = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)img;

                // -------------------------------------------------
                // 1. Create a raster image definition for the background
                // -------------------------------------------------
                var rasterDef = new CadRasterImageDef
                {
                    FileName = backgroundPath,
                    ObjectHandle = Guid.NewGuid().ToString()
                };

                // Preserve existing objects and add the new definition
                var objectsList = cadImage.Objects?.ToList() ?? new List<CadBaseObject>();
                objectsList.Add(rasterDef);
                cadImage.Objects = objectsList.ToArray();

                // -------------------------------------------------
                // 2. Create a raster image entity that references the definition
                // -------------------------------------------------
                var insertionPoint = new Cad3DPoint(0, 0, 0);
                var uVector = new Cad3DPoint(1, 0, 0); // X direction, scale = 1
                var vVector = new Cad3DPoint(0, 1, 0); // Y direction, scale = 1

                var rasterImage = new CadRasterImage(rasterDef, insertionPoint, uVector, vVector)
                {
                    ImageDefReference = rasterDef.ObjectHandle
                };

                // Preserve existing entities and add the raster image entity
                var entitiesList = cadImage.Entities?.ToList() ?? new List<CadEntityBase>();
                entitiesList.Add(rasterImage);
                cadImage.Entities = entitiesList.ToArray();

                // -------------------------------------------------
                // 3. (Optional) Adjust hatch origins inside each block if hatch entities are present
                // -------------------------------------------------
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    foreach (var entity in block.Entities)
                    {
                        // Attempt to adjust origin for hatch-like entities via reflection
                        var type = entity.GetType();
                        var originProp = type.GetProperty("Origin");
                        if (originProp != null && originProp.CanRead && originProp.CanWrite)
                        {
                            var cur = originProp.GetValue(entity);
                            if (cur != null)
                            {
                                var xProp = cur.GetType().GetProperty("X");
                                var yProp = cur.GetType().GetProperty("Y");
                                if (xProp != null && yProp != null)
                                {
                                    float x = Convert.ToSingle(xProp.GetValue(cur));
                                    float y = Convert.ToSingle(yProp.GetValue(cur));
                                    var pointType = cur.GetType();
                                    var newPoint = Activator.CreateInstance(pointType, x + 10f, y + 10f);
                                    originProp.SetValue(entity, newPoint);
                                }
                            }
                        }
                    }
                }

                // -------------------------------------------------
                // 4. Save the modified drawing as PNG
                // -------------------------------------------------
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = new CadRasterizationOptions
                    {
                        UnitType = UnitType.Centimenter,
                        PageWidth = 1000f,
                        PageHeight = 1000f,
                        BackgroundColor = Aspose.CAD.Color.White
                    }
                };

                cadImage.Save(outputPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}