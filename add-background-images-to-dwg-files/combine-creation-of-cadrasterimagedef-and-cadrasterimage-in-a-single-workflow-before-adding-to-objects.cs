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
        try
        {
            string inputDwGPath = "input.dwg";
            string rasterImagePath = "background.png";

            if (!File.Exists(inputDwGPath))
            {
                Console.Error.WriteLine($"Input DWG file not found: {inputDwGPath}");
                return;
            }

            if (!File.Exists(rasterImagePath))
            {
                Console.Error.WriteLine($"Raster image file not found: {rasterImagePath}");
                return;
            }

            using (CadImage cadImage = (CadImage)Image.Load(inputDwGPath))
            {
                // Load raster image to get dimensions
                int imgWidth, imgHeight;
                using (Image rasterImg = Image.Load(rasterImagePath))
                {
                    imgWidth = rasterImg.Width;
                    imgHeight = rasterImg.Height;
                }

                // Create raster image definition
                CadRasterImageDef rasterDef = new CadRasterImageDef(rasterImagePath, imgWidth, imgHeight)
                {
                    ObjectHandle = Guid.NewGuid().ToString("N")
                };

                // Add definition to objects collection
                var objects = cadImage.Objects?.ToList() ?? new List<CadBaseObject>();
                objects.Add(rasterDef);
                cadImage.Objects = objects.ToArray();

                // Define insertion point and orientation vectors
                Cad3DPoint insertionPoint = new Cad3DPoint(0, 0, 0);
                Cad3DPoint uVector = new Cad3DPoint(imgWidth, 0, 0);
                Cad3DPoint vVector = new Cad3DPoint(0, imgHeight, 0);

                // Create raster image entity linked to definition
                CadRasterImage rasterImage = new CadRasterImage(rasterDef, insertionPoint, uVector, vVector);

                // Add raster image to entities collection
                var entities = cadImage.Entities?.ToList() ?? new List<CadEntityBase>();
                entities.Add(rasterImage);
                cadImage.Entities = entities.ToArray();

                // Save modified DWG
                string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
                Directory.CreateDirectory(outputDir);
                string outputPath = Path.Combine(outputDir, "with_background.dwg");
                cadImage.Save(outputPath);

                Console.WriteLine($"DWG file saved with background image at: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
