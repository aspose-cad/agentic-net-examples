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
        string inputPath = "input.dwg";
        string outputPath = "output.dwg";
        string backgroundImagePath = "background.png";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input DWG file not found: {inputPath}");
            return;
        }

        if (!File.Exists(backgroundImagePath))
        {
            Console.WriteLine($"Background image file not found: {backgroundImagePath}");
            return;
        }

        try
        {
            // Load the DWG file
            using (Image dwgImg = Image.Load(inputPath))
            {
                CadImage cadImage = dwgImg as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Load background image to obtain its dimensions
                int imgWidth, imgHeight;
                using (Image bgImg = Image.Load(backgroundImagePath))
                {
                    imgWidth = bgImg.Width;
                    imgHeight = bgImg.Height;
                }

                // Create raster image definition
                CadRasterImageDef rasterDef = new CadRasterImageDef(backgroundImagePath, imgWidth, imgHeight);

                // Add definition to Objects collection
                var objectsList = cadImage.Objects?.ToList() ?? new List<CadBaseObject>();
                objectsList.Add(rasterDef);
                cadImage.Objects = objectsList.ToArray();

                // Ensure the definition has a handle
                string defHandle = rasterDef.ObjectHandle ?? "1F";

                // Create raster image entity referencing the definition
                var insertionPoint = new Cad3DPoint(0, 0, 0);
                var uVector = new Cad3DPoint(imgWidth, 0, 0);
                var vVector = new Cad3DPoint(0, imgHeight, 0);
                CadRasterImage rasterImage = new CadRasterImage(rasterDef, insertionPoint, uVector, vVector)
                {
                    ImageDefReference = defHandle
                };

                // Add raster image entity to Entities collection
                var entitiesList = cadImage.Entities?.ToList() ?? new List<CadEntityBase>();
                entitiesList.Add(rasterImage);
                cadImage.Entities = entitiesList.ToArray();

                // Save the modified DWG
                DwgOptions saveOptions = new DwgOptions();
                cadImage.Save(outputPath, saveOptions);
                Console.WriteLine($"Modified DWG saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing DWG: {ex.Message}");
        }
    }
}
