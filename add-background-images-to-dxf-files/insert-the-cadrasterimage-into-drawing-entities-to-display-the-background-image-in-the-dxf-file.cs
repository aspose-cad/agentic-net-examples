using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
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
            // Paths to the source DXF file and the background raster image.
            string dxfPath = @"Input.dxf";
            string backgroundImagePath = @"Background.png";
            string outputPath = @"Output\Output.dxf";

            // Verify input files exist.
            if (!File.Exists(dxfPath))
            {
                Console.Error.WriteLine($"DXF file not found: {dxfPath}");
                return;
            }
            if (!File.Exists(backgroundImagePath))
            {
                Console.Error.WriteLine($"Background image not found: {backgroundImagePath}");
                return;
            }

            // Ensure output directory exists.
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
                Directory.CreateDirectory(outputDir);

            // Load the DXF drawing.
            using (Image dxfImage = Image.Load(dxfPath))
            {
                // Cast to CadImage to access CAD-specific members.
                CadImage cadImage = (CadImage)dxfImage;

                // Load background image to obtain its dimensions.
                int imgWidth, imgHeight;
                using (Image bgImg = Image.Load(backgroundImagePath))
                {
                    imgWidth = bgImg.Width;
                    imgHeight = bgImg.Height;
                }

                // Create a raster image definition that references the external image file.
                CadRasterImageDef rasterDef = new CadRasterImageDef(backgroundImagePath, imgWidth, imgHeight);
                // Assign a unique handle for reference (simple GUID based handle).
                rasterDef.ObjectHandle = Guid.NewGuid().ToString("N").ToUpper();

                // Add the raster definition to the drawing's objects collection.
                var objectsList = cadImage.Objects?.ToList() ?? new List<CadBaseObject>();
                objectsList.Add(rasterDef);
                cadImage.Objects = objectsList.ToArray();

                // Create the raster image entity that uses the definition.
                Cad3DPoint insertionPoint = new Cad3DPoint(0, 0, 0);
                Cad3DPoint uVector = new Cad3DPoint(imgWidth, 0, 0);   // Width along X
                Cad3DPoint vVector = new Cad3DPoint(0, imgHeight, 0); // Height along Y

                CadRasterImage rasterEntity = new CadRasterImage(rasterDef, insertionPoint, uVector, vVector);
                rasterEntity.ImageDefReference = rasterDef.ObjectHandle;

                // Add the raster entity to the drawing's top‑level entities collection.
                var entitiesList = cadImage.Entities?.ToList() ?? new List<CadEntityBase>();
                entitiesList.Add(rasterEntity);
                cadImage.Entities = entitiesList.ToArray();

                // Save the modified drawing back to DXF format.
                DxfOptions saveOptions = new DxfOptions();
                cadImage.Save(outputPath);
            }

            Console.WriteLine("Background image inserted and DXF saved to: " + Path.GetFullPath(outputPath));
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}