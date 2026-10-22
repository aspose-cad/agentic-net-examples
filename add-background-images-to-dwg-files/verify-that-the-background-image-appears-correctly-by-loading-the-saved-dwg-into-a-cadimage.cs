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
        try
        {
            // Input DWG and background image paths
            string sourceDwgPath = "input.dwg";
            string backgroundImagePath = "background.png";
            string outputDwgPath = "output.dwg";

            // Verify that the required files exist
            if (!File.Exists(sourceDwgPath))
            {
                Console.Error.WriteLine($"Source DWG file not found: {sourceDwgPath}");
                return;
            }
            if (!File.Exists(backgroundImagePath))
            {
                Console.Error.WriteLine($"Background image file not found: {backgroundImagePath}");
                return;
            }

            // Load the DWG file
            using (CadImage dwgImage = (CadImage)Image.Load(sourceDwgPath))
            {
                // Parameters for the background image
                double x = 0;
                double y = 0;
                double width = 500;   // example width
                double height = 500;  // example height

                // Create a raster image definition
                var rasterDef = new CadRasterImageDef
                {
                    FileName = backgroundImagePath,
                    ImageSizeU = width,
                    ImageSizeV = height,
                    ObjectHandle = "5000"
                };

                // Add the definition to the Objects collection
                var objectsList = dwgImage.Objects?.ToList() ?? new List<CadBaseObject>();
                objectsList.Add(rasterDef);
                dwgImage.Objects = objectsList.ToArray();

                // Create the raster image entity that references the definition
                var insertionPoint = new Cad3DPoint(x, y, 0);
                var uVector = new Cad3DPoint(width, 0, 0);
                var vVector = new Cad3DPoint(0, height, 0);

                var rasterImage = new CadRasterImage(rasterDef, insertionPoint, uVector, vVector)
                {
                    ImageDefReference = rasterDef.ObjectHandle,
                    ImageSizeU = width,
                    ImageSizeV = height
                };

                // Add the raster image entity to the Entities collection
                var entitiesList = dwgImage.Entities?.ToList() ?? new List<CadEntityBase>();
                entitiesList.Add(rasterImage);
                dwgImage.Entities = entitiesList.ToArray();

                // Save the modified DWG file
                dwgImage.Save(outputDwgPath, new DwgOptions());
            }

            // Verify that the saved DWG can be loaded correctly
            if (!File.Exists(outputDwgPath))
            {
                Console.Error.WriteLine($"Saved DWG file not found: {outputDwgPath}");
                return;
            }

            using (CadImage verifyImage = (CadImage)Image.Load(outputDwgPath))
            {
                int blockCount = verifyImage.BlockEntities.Count;
                Console.WriteLine($"Verification completed. Block entity count: {blockCount}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
