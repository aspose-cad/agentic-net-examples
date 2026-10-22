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
        const string sourceFile = "sample.dwg";
        const string rasterFile = "background.png";
        const string outputFolder = "Output";
        const string outputFile = "result.png";

        if (!File.Exists(sourceFile))
        {
            Console.Error.WriteLine($"Source DWG file not found: {sourceFile}");
            return;
        }

        if (!File.Exists(rasterFile))
        {
            Console.Error.WriteLine($"Raster image file not found: {rasterFile}");
            return;
        }

        try
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            using (Image image = Image.Load(sourceFile))
            {
                var cadImage = (CadImage)image;

                // Create raster image definition
                var rasterDef = new CadRasterImageDef
                {
                    FileName = rasterFile,
                    ImageSizeU = 100.0,
                    ImageSizeV = 100.0
                };

                // Create raster image entity
                var insertionPoint = new Cad3DPoint(0.0, 0.0, 0.0);
                var uVector = new Cad3DPoint(100.0, 0.0, 0.0);
                var vVector = new Cad3DPoint(0.0, 100.0, 0.0);
                var rasterImage = new CadRasterImage(rasterDef, insertionPoint, uVector, vVector);

                // Add definition to Objects collection
                var objectsList = cadImage.Objects.ToList();
                objectsList.Add(rasterDef);
                cadImage.Objects = objectsList.ToArray();

                // Add raster image entity to Entities collection
                var entitiesList = cadImage.Entities.ToList();
                entitiesList.Add(rasterImage);
                cadImage.Entities = entitiesList.ToArray();

                // Validation
                bool containsRasterDef = cadImage.Objects.Any(o => o is CadRasterImageDef);
                bool containsOtherObject = cadImage.Objects.Any(o => !(o is CadRasterImageDef));
                bool containsRasterEntity = cadImage.Entities.Any(e => e is CadRasterImage);

                if (containsRasterDef && containsOtherObject && containsRasterEntity)
                {
                    Console.WriteLine("Validation succeeded: Objects contain a raster definition and other objects, and Entities contain a raster image.");
                }
                else
                {
                    Console.Error.WriteLine("Validation failed: Required object types are missing.");
                }

                // Save as PNG
                string outputPath = Path.Combine(outputFolder, outputFile);
                var pngOptions = new PngOptions();
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"File saved successfully to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
