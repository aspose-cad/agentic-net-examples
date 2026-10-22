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
            string dwgPath = "input.dwg";
            string rasterPath = "background.png";
            string outputPath = "output.dwg";

            if (!File.Exists(dwgPath))
            {
                Console.Error.WriteLine($"DWG file not found: {dwgPath}");
                return;
            }

            if (!File.Exists(rasterPath))
            {
                Console.Error.WriteLine($"Raster image file not found: {rasterPath}");
                return;
            }

            using (CadImage cadImage = (CadImage)Image.Load(dwgPath))
            {
                // Load raster image to obtain its dimensions
                using (Image rasterImg = Image.Load(rasterPath))
                {
                    int width = rasterImg.Width;
                    int height = rasterImg.Height;

                    // Create raster image definition
                    CadRasterImageDef rasterDef = new CadRasterImageDef(rasterPath, width, height);
                    // Ensure the definition has a unique handle (optional, Aspose may assign automatically)
                    rasterDef.ObjectHandle = Guid.NewGuid().ToString("N");

                    // Add definition to the drawing objects collection
                    var objects = new List<CadBaseObject>(cadImage.Objects ?? Array.Empty<CadBaseObject>());
                    objects.Add(rasterDef);
                    cadImage.Objects = objects.ToArray();

                    // Create raster image entity referencing the definition
                    Cad3DPoint insertionPoint = new Cad3DPoint(0, 0, 0);
                    Cad3DPoint uVector = new Cad3DPoint(width, 0, 0);
                    Cad3DPoint vVector = new Cad3DPoint(0, height, 0);
                    CadRasterImage rasterImage = new CadRasterImage(rasterDef, insertionPoint, uVector, vVector);

                    // Add raster image entity to the drawing entities collection
                    var entities = new List<CadEntityBase>(cadImage.Entities ?? Array.Empty<CadEntityBase>());
                    entities.Add(rasterImage);
                    cadImage.Entities = entities.ToArray();
                }

                // Save the modified DWG
                cadImage.Save(outputPath, new DwgOptions());
            }

            Console.WriteLine("Background image added and saved successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
