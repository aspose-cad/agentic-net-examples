using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

namespace AddBackgroundImageExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Paths (replace with actual paths or placeholders)
                string sourceDwGPath = "input.dwg";
                string backgroundImagePath = "background.png";
                string outputDwGPath = "output.dwg";

                if (!File.Exists(sourceDwGPath))
                {
                    Console.WriteLine($"Source DWG file not found: {sourceDwGPath}");
                    return;
                }

                if (!File.Exists(backgroundImagePath))
                {
                    Console.WriteLine($"Background image file not found: {backgroundImagePath}");
                    return;
                }

                // Load the DWG file
                using (CadImage cadImage = (CadImage)Image.Load(sourceDwGPath))
                {
                    // Create raster image definition
                    // Width and Height are set to 0; actual size will be defined by the entity vectors
                    var rasterDef = new CadRasterImageDef(backgroundImagePath, 0, 0);

                    // Insert the definition at the beginning of the Objects collection
                    var objectsList = cadImage.Objects?.ToList() ?? new List<CadBaseObject>();
                    objectsList.Insert(0, rasterDef);
                    cadImage.Objects = objectsList.ToArray();

                    // Define placement vectors (adjust size as needed)
                    double imageWidth = 500;   // drawing units
                    double imageHeight = 500;  // drawing units

                    var insertionPoint = new Cad3DPoint(0, 0, 0);
                    var uVector = new Cad3DPoint(imageWidth, 0, 0);
                    var vVector = new Cad3DPoint(0, imageHeight, 0);

                    // Create raster image entity referencing the definition
                    var backgroundEntity = new CadRasterImage(rasterDef, insertionPoint, uVector, vVector)
                    {
                        LayerName = "0",
                        ImageDefReference = rasterDef.ObjectHandle
                    };

                    // Insert the raster image entity at the beginning of the Entities collection (background layer)
                    var entitiesList = cadImage.Entities?.ToList() ?? new List<CadEntityBase>();
                    entitiesList.Insert(0, backgroundEntity);
                    cadImage.Entities = entitiesList.ToArray();

                    // Save the modified drawing
                    var dwgSaveOptions = new DwgOptions();
                    cadImage.Save(outputDwGPath, dwgSaveOptions);
                }

                Console.WriteLine($"Background image added and saved to: {outputDwGPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}