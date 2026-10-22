using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
                // Input DWG file (placeholder)
                string inputPath = "input.dwg";
                // Output directory (placeholder)
                string outputDir = "output";

                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Ensure output directory exists
                Directory.CreateDirectory(outputDir);

                // Placeholder background image files
                string[] backgroundImages = new string[]
                {
                    "background1.png",
                    "background2.png",
                    "background3.png"
                };

                // Load the DWG file as a CadImage
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Prepare collections for objects and entities
                    List<CadBaseObject> objects = cadImage.Objects?.ToList() ?? new List<CadBaseObject>();
                    List<CadEntityBase> entities = cadImage.Entities?.ToList() ?? new List<CadEntityBase>();

                    for (int i = 0; i < backgroundImages.Length; i++)
                    {
                        string bgPath = backgroundImages[i];

                        // Create a raster image definition
                        CadRasterImageDef rasterDef = new CadRasterImageDef
                        {
                            FileName = bgPath,
                            ImageSizeU = 100.0, // placeholder size
                            ImageSizeV = 100.0,
                            ObjectHandle = $"-1{i}" // unique handle
                        };

                        // Add definition to objects collection
                        objects.Add(rasterDef);

                        // Create a raster image entity that references the definition
                        CadRasterImage rasterImage = new CadRasterImage
                        {
                            ImageDefReference = rasterDef.ObjectHandle,
                            InsertionPoint = new Cad3DPoint(0, 0, 0),
                            UVector = new Cad3DPoint(100, 0, 0),
                            VVector = new Cad3DPoint(0, 100, 0),
                            ImageSizeU = rasterDef.ImageSizeU,
                            ImageSizeV = rasterDef.ImageSizeV
                        };

                        // Add entity to entities collection
                        entities.Add(rasterImage);
                    }

                    // Assign the updated collections back to the image
                    cadImage.Objects = objects.ToArray();
                    cadImage.Entities = entities.ToArray();

                    // Save the modified DWG
                    string outputPath = Path.Combine(outputDir, "output.dwg");
                    cadImage.Save(outputPath);
                    Console.WriteLine($"DWG saved with background images to: {outputPath}");
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}
