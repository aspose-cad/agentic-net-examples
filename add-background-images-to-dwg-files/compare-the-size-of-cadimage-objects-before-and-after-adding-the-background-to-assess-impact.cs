using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace AddBackgroundImageExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Paths to the source DWG file, background image, and output DWG file.
            string sourceDwgPath = "input.dwg";               // TODO: replace with actual path
            string backgroundImagePath = "background.png";   // TODO: replace with actual path
            string outputDwgPath = "output.dwg";              // TODO: replace with desired output path

            // Verify source files exist
            if (!File.Exists(sourceDwgPath))
            {
                Console.WriteLine($"Source DWG file not found: {sourceDwgPath}");
                return;
            }

            if (!File.Exists(backgroundImagePath))
            {
                Console.WriteLine($"Background image file not found: {backgroundImagePath}");
                return;
            }

            try
            {
                // Load the DWG file.
                using (CadImage cadImage = (CadImage)Image.Load(sourceDwgPath))
                {
                    // Count objects before adding the background.
                    int countBefore = cadImage.Objects?.Length ?? 0;
                    Console.WriteLine($"Object count before adding background: {countBefore}");

                    // Load background image to obtain its dimensions.
                    using (Image bgImg = Image.Load(backgroundImagePath))
                    {
                        double width = bgImg.Width;
                        double height = bgImg.Height;

                        // Create a raster image definition.
                        CadRasterImageDef rasterDef = new CadRasterImageDef(backgroundImagePath, (int)width, (int)height)
                        {
                            // Generate a unique handle for the definition.
                            ObjectHandle = Guid.NewGuid().ToString("N")
                        };

                        // Preserve existing objects and add the new definition.
                        List<CadBaseObject> objects = cadImage.Objects?.ToList() ?? new List<CadBaseObject>();
                        objects.Add(rasterDef);
                        cadImage.Objects = objects.ToArray();

                        // Create the raster image entity that references the definition.
                        Cad3DPoint insertionPoint = new Cad3DPoint(0, 0, 0);
                        Cad3DPoint uVector = new Cad3DPoint(width, 0, 0);
                        Cad3DPoint vVector = new Cad3DPoint(0, height, 0);

                        CadRasterImage rasterImage = new CadRasterImage(rasterDef, insertionPoint, uVector, vVector)
                        {
                            ImageDefReference = rasterDef.ObjectHandle,
                            ImageSizeU = width,
                            ImageSizeV = height
                        };

                        // Preserve existing entities and add the new raster image.
                        List<CadEntityBase> entities = cadImage.Entities?.ToList() ?? new List<CadEntityBase>();
                        entities.Add(rasterImage);
                        cadImage.Entities = entities.ToArray();
                    }

                    // Count objects after adding the background.
                    int countAfter = cadImage.Objects?.Length ?? 0;
                    Console.WriteLine($"Object count after adding background: {countAfter}");

                    // Save the modified drawing.
                    cadImage.Save(outputDwgPath);
                    Console.WriteLine($"Modified DWG saved to: {outputDwgPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
