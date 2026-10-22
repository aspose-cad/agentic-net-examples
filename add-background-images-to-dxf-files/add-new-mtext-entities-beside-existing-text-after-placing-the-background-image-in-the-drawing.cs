using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Paths (replace with actual paths as needed)
            string inputPath = "input.dxf";
            string backgroundImagePath = "background.png";
            string outputPath = "output.dxf";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            if (!File.Exists(backgroundImagePath))
            {
                Console.WriteLine($"Background image not found: {backgroundImagePath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Load DXF file
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Get Model Space block
                CadBlockEntity modelSpace = cadImage.BlockEntities["*Model_Space"];

                // -------------------------------------------------
                // Add background raster image
                // -------------------------------------------------
                // Create raster image definition
                CadRasterImageDef rasterDef = new CadRasterImageDef
                {
                    FileName = backgroundImagePath,
                    ImageSizeU = 500, // width in drawing units
                    ImageSizeV = 500, // height in drawing units
                    ObjectHandle = "5F" // unique handle (choose any unused value)
                };

                // Add definition to the CAD objects collection
                var objectsList = new List<CadBaseObject>(cadImage.Objects ?? Array.Empty<CadBaseObject>());
                objectsList.Add(rasterDef);
                cadImage.Objects = objectsList.ToArray();

                // Create raster image entity referencing the definition
                var insertionPoint = new Cad3DPoint(0, 0, 0);
                var uVector = new Cad3DPoint(500, 0, 0); // width direction
                var vVector = new Cad3DPoint(0, 500, 0); // height direction

                CadRasterImage rasterEntity = new CadRasterImage(rasterDef, insertionPoint, uVector, vVector)
                {
                    ImageDefReference = rasterDef.ObjectHandle
                };

                // Add raster entity to Model Space
                modelSpace.AddEntity(rasterEntity);

                // -------------------------------------------------
                // Add MTEXT entities beside existing MTEXT entities
                // -------------------------------------------------
                // Iterate over a copy of the current entities to avoid modification during enumeration
                var existingEntities = new List<CadEntityBase>(modelSpace.Entities);
                foreach (var entity in existingEntities)
                {
                    if (entity is CadMText existingMText)
                    {
                        var newMText = new CadMText
                        {
                            Text = "Added Text",
                            InsertionPoint = new Cad3DPoint(
                                existingMText.InsertionPoint.X + 10,
                                existingMText.InsertionPoint.Y,
                                existingMText.InsertionPoint.Z),
                            InitialTextHeight = existingMText.InitialTextHeight,
                            RotationAngleRad = existingMText.RotationAngleRad
                        };

                        modelSpace.AddEntity(newMText);
                    }
                }

                // Save the modified DXF
                cadImage.Save(outputPath, new DxfOptions());
            }

            Console.WriteLine("DXF file processed and saved successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}