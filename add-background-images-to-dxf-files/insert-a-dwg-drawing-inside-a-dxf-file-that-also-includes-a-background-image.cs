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
        string dxfPath = "input.dxf";
        string dwgPath = "insert.dwg";
        string backgroundImagePath = "background.png";
        string outputPath = "output.dxf";

        if (!File.Exists(dxfPath) || !File.Exists(dwgPath) || !File.Exists(backgroundImagePath))
        {
            Console.WriteLine("One or more input files are missing. Operation aborted.");
            return;
        }

        try
        {
            using (Image dxfImage = Image.Load(dxfPath))
            using (Image dwgImage = Image.Load(dwgPath))
            {
                CadImage dxfCad = dxfImage as CadImage;
                CadImage dwgCad = dwgImage as CadImage;

                if (dxfCad == null || dwgCad == null)
                {
                    Console.WriteLine("Failed to cast loaded images to CadImage. Operation aborted.");
                    return;
                }

                // 1. Create a new block in the DXF that contains DWG entities
                string blockName = "InsertedDWGBlock";

                CadBlockEntity newBlock = new CadBlockEntity
                {
                    BasePoint = new Cad3DPoint(0, 0, 0)
                };

                foreach (CadEntityBase entity in dwgCad.Entities)
                {
                    newBlock.AddEntity(entity);
                }

                dxfCad.BlockEntities.Add(blockName, newBlock);

                // 2. Insert the newly created block into the DXF drawing
                CadInsertObject insertObject = new CadInsertObject
                {
                    InsertionPoint = new Cad3DPoint(10, 10, 0),
                    Name = blockName,
                    ScaleX = 1,
                    ScaleY = 1,
                    ScaleZ = 1,
                    RotationAngle = 0
                };

                dxfCad.Entities = dxfCad.Entities.Concat(new CadEntityBase[] { insertObject }).ToArray();

                // 3. Add a background raster image to the DXF
                // Create raster image definition
                CadRasterImageDef rasterDef = new CadRasterImageDef
                {
                    FileName = backgroundImagePath,
                    ImageSizeU = 200, // width in drawing units
                    ImageSizeV = 200, // height in drawing units
                    ObjectHandle = "5A" // any unique handle
                };

                // Add definition to Objects collection
                var objectsList = dxfCad.Objects?.ToList() ?? new List<CadBaseObject>();
                objectsList.Add(rasterDef);
                dxfCad.Objects = objectsList.ToArray();

                // Create raster image entity referencing the definition
                CadRasterImage rasterImage = new CadRasterImage(
                    rasterDef,
                    new Cad3DPoint(0, 0, 0),               // Insertion point
                    new Cad3DPoint(200, 0, 0),             // U vector (width direction)
                    new Cad3DPoint(0, 200, 0)              // V vector (height direction)
                )
                {
                    ImageDefReference = rasterDef.ObjectHandle
                };

                dxfCad.Entities = dxfCad.Entities.Concat(new CadEntityBase[] { rasterImage }).ToArray();

                // 4. Save the modified DXF file
                DxfOptions saveOptions = new DxfOptions();
                dxfCad.Save(outputPath, saveOptions);
            }

            Console.WriteLine($"DXF file saved successfully to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
