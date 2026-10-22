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
        static void Main()
        {
            try
            {
                // Input DWG file (must exist)
                string inputPath = "input.dwg";
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Raster image to embed (must exist)
                string rasterPath = "background.png";
                if (!File.Exists(rasterPath))
                {
                    Console.Error.WriteLine($"Raster image file not found: {rasterPath}");
                    return;
                }

                // Prepare output directory
                string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
                Directory.CreateDirectory(outputDir);
                string outputPath = Path.Combine(outputDir, "output_with_background.dxf");

                // Load the DWG file
                using (Image img = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD specific members
                    CadImage cadImage = (CadImage)img;

                    // Define arbitrary image size in drawing units (e.g., 200 x 150)
                    double imageSizeU = 200.0;
                    double imageSizeV = 150.0;

                    // Create raster image definition
                    CadRasterImageDef rasterDef = new CadRasterImageDef(rasterPath, 0, 0)
                    {
                        ImageSizeU = imageSizeU,
                        ImageSizeV = imageSizeV,
                        ObjectHandle = "R" + Guid.NewGuid().ToString("N")
                    };

                    // Add definition to the Objects collection (preserve existing objects)
                    List<CadBaseObject> objects = cadImage.Objects?.ToList() ?? new List<CadBaseObject>();
                    objects.Add(rasterDef);
                    cadImage.Objects = objects.ToArray();

                    // Define insertion point and vectors
                    Cad3DPoint insertionPoint = new Cad3DPoint(0, 0, 0);
                    Cad3DPoint uVector = new Cad3DPoint(imageSizeU, 0, 0);
                    Cad3DPoint vVector = new Cad3DPoint(0, imageSizeV, 0);

                    // Create raster image entity referencing the definition
                    CadRasterImage rasterImage = new CadRasterImage(rasterDef, insertionPoint, uVector, vVector);

                    // Add raster image entity to the Entities collection
                    List<CadEntityBase> entities = cadImage.Entities?.ToList() ?? new List<CadEntityBase>();
                    entities.Add(rasterImage);
                    cadImage.Entities = entities.ToArray();

                    // Save the modified drawing (as DXF)
                    DxfOptions saveOptions = new DxfOptions();
                    cadImage.Save(outputPath, saveOptions);
                }

                Console.WriteLine($"Drawing saved successfully to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
