using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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
            string inputPath = "input.dwg";
            string outputPath = "output.dwg";
            string backgroundImagePath = "background.png";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input DWG file not found: {inputPath}");
                return;
            }

            if (!File.Exists(backgroundImagePath))
            {
                Console.Error.WriteLine($"Background image file not found: {backgroundImagePath}");
                return;
            }

            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage for CAD-specific operations
                CadImage cadImage = (CadImage)image;

                // Get the Model Space block where drawing entities are stored
                if (!cadImage.BlockEntities.TryGetValue("*Model_Space", out var blockObj) || !(blockObj is CadBlockEntity modelSpace))
                {
                    Console.Error.WriteLine("Model space block not found.");
                    return;
                }

                // Create a simple line (CadBaseObject) and add it to the drawing
                CadLine line = new CadLine
                {
                    FirstPoint = new Cad3DPoint(0, 0, 0),
                    SecondPoint = new Cad3DPoint(100, 100, 0)
                };
                modelSpace.AddEntity(line);

                // Create a raster image definition
                CadRasterImageDef rasterDef = new CadRasterImageDef
                {
                    FileName = backgroundImagePath,
                    ObjectHandle = "5F" // unique handle identifier
                };

                // Add the definition to the Objects collection
                var objectsList = new List<CadBaseObject>(cadImage.Objects ?? Array.Empty<CadBaseObject>());
                objectsList.Add(rasterDef);
                cadImage.Objects = objectsList.ToArray();

                // Create a raster image entity that references the definition
                CadRasterImage rasterImage = new CadRasterImage
                {
                    ImageDefReference = rasterDef.ObjectHandle,
                    InsertionPoint = new Cad3DPoint(0, 0, 0),
                    UVector = new Cad3DPoint(1, 0, 0), // scale in X direction
                    VVector = new Cad3DPoint(0, 1, 0)  // scale in Y direction
                };

                // Add the raster image entity to model space
                modelSpace.AddEntity(rasterImage);

                // Save the modified DWG file
                cadImage.Save(outputPath, new DwgOptions());
            }

            Console.WriteLine("DWG file updated and saved successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
