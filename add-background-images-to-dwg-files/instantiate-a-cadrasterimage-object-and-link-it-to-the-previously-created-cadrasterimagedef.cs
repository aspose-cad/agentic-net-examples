using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

namespace AddBackgroundImageToDWG
{
    class Program
    {
        static void Main()
        {
            // Paths (replace with actual paths or placeholders)
            string dwgPath = "input.dwg";
            string backgroundImagePath = "background.png";
            string outputPath = "output.dwg";

            try
            {
                // Validate input files
                if (!File.Exists(dwgPath))
                    throw new FileNotFoundException($"DWG file not found: {dwgPath}");
                if (!File.Exists(backgroundImagePath))
                    throw new FileNotFoundException($"Background image not found: {backgroundImagePath}");

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    Directory.CreateDirectory(outputDir);

                // Get background image dimensions using Aspose.CAD
                int imgWidth, imgHeight;
                using (var bgImg = Image.Load(backgroundImagePath))
                {
                    imgWidth = bgImg.Width;
                    imgHeight = bgImg.Height;
                }

                // Load the DWG file
                using (var cadImage = Image.Load(dwgPath) as CadImage)
                {
                    if (cadImage == null)
                        throw new InvalidOperationException("Failed to load DWG as CadImage.");

                    // Create raster image definition
                    var rasterDef = new CadRasterImageDef(backgroundImagePath, imgWidth, imgHeight);

                    // Add definition to Objects collection
                    var objects = cadImage.Objects?.ToList() ?? new List<CadBaseObject>();
                    objects.Add(rasterDef);
                    cadImage.Objects = objects.ToArray();

                    // Create raster image entity
                    var insertionPoint = new Cad3DPoint(0, 0, 0);
                    var uVector = new Cad3DPoint(imgWidth, 0, 0);
                    var vVector = new Cad3DPoint(0, imgHeight, 0);
                    var rasterImage = new CadRasterImage(rasterDef, insertionPoint, uVector, vVector);

                    // Add entity to Entities collection
                    var entities = cadImage.Entities?.ToList() ?? new List<CadEntityBase>();
                    entities.Add(rasterImage);
                    cadImage.Entities = entities.ToArray();

                    // Save the modified DWG using DxfOptions (DWG saving not directly supported)
                    cadImage.Save(outputPath, new DxfOptions());
                }

                Console.WriteLine("Background image added and DWG saved successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
