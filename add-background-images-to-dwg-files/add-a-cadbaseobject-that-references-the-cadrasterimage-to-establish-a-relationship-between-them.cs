using System;
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
            string inputPath = "input.dwg";
            string rasterPath = "background.png";
            string outputDir = "Output";
            string outputPath = Path.Combine(outputDir, "output_with_background.dwg");

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input CAD file not found: {inputPath}");
                return;
            }

            if (!File.Exists(rasterPath))
            {
                Console.Error.WriteLine($"Raster image file not found: {rasterPath}");
                return;
            }

            Directory.CreateDirectory(outputDir);

            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Create raster image definition
                var rasterDef = new CadRasterImageDef
                {
                    FileName = rasterPath,
                    ImageSizeU = 500,
                    ImageSizeV = 400,
                    ObjectHandle = Guid.NewGuid().ToString()
                };

                // Add definition to the Objects collection, preserving existing objects
                var existingObjects = cadImage.Objects ?? new CadBaseObject[0];
                cadImage.Objects = existingObjects.Concat(new CadBaseObject[] { rasterDef }).ToArray();

                // Create raster image entity referencing the definition
                var insertionPoint = new Cad3DPoint(0, 0, 0);
                var uVector = new Cad3DPoint(500, 0, 0);
                var vVector = new Cad3DPoint(0, 400, 0);
                var rasterEntity = new CadRasterImage(rasterDef, insertionPoint, uVector, vVector);

                // Add raster entity to the Entities collection
                var existingEntities = cadImage.Entities ?? new CadEntityBase[0];
                cadImage.Entities = existingEntities.Concat(new CadEntityBase[] { rasterEntity }).ToArray();

                // Save the modified drawing
                cadImage.Save(outputPath, new DwgOptions());
            }

            Console.WriteLine($"Output saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}
