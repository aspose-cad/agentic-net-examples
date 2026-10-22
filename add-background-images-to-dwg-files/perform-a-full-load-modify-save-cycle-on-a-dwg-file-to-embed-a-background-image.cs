using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string backgroundPath = "background.png";
        string outputPath = "output.dwg";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input DWG file not found: {inputPath}");
            return;
        }

        if (!File.Exists(backgroundPath))
        {
            Console.WriteLine($"Background image file not found: {backgroundPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            using (Image loadedImage = Image.Load(inputPath))
            {
                if (loadedImage is not CadImage cadImage)
                {
                    Console.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Copy background image to a temporary location (Aspose.CAD expects a file path)
                string tempImagePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + Path.GetExtension(backgroundPath));
                File.Copy(backgroundPath, tempImagePath, true);

                // Create raster image definition
                var rasterDef = new CadRasterImageDef
                {
                    FileName = tempImagePath,
                    ObjectHandle = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()
                };

                // Add definition to Objects collection
                var objectsList = cadImage.Objects?.ToList() ?? new List<CadBaseObject>();
                objectsList.Add(rasterDef);
                cadImage.Objects = objectsList.ToArray();

                // Define insertion point and orientation vectors
                var insertionPoint = new Cad3DPoint(0, 0, 0);
                var uVector = new Cad3DPoint(1, 0, 0);
                var vVector = new Cad3DPoint(0, 1, 0);

                // Create raster image entity
                var rasterImage = new CadRasterImage(rasterDef, insertionPoint, uVector, vVector)
                {
                    ImageDefReference = rasterDef.ObjectHandle
                };

                // Add raster entity to Entities collection
                var entitiesList = cadImage.Entities?.ToList() ?? new List<CadEntityBase>();
                entitiesList.Add(rasterImage);
                cadImage.Entities = entitiesList.ToArray();

                // Save modified DWG
                cadImage.Save(outputPath);
                Console.WriteLine($"DWG saved successfully: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
