using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace AddBackgroundImages
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Input DWG file
                string dwgPath = "input.dwg";
                if (!File.Exists(dwgPath))
                {
                    Console.Error.WriteLine($"DWG file not found: {dwgPath}");
                    return;
                }

                // Background raster images to add (placeholders)
                string[] rasterPaths = { "bg1.png", "bg2.jpg" };
                foreach (string rp in rasterPaths)
                {
                    if (!File.Exists(rp))
                    {
                        Console.Error.WriteLine($"Raster image not found: {rp}");
                        return;
                    }
                }

                // Load DWG as CadImage
                using (Image dwgImage = Image.Load(dwgPath))
                {
                    CadImage cadImage = dwgImage as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // Get the Model Space block where entities are stored
                    const string modelSpaceName = "*Model_Space";
                    if (!cadImage.BlockEntities.ContainsKey(modelSpaceName))
                    {
                        Console.Error.WriteLine("Model space block not found in DWG.");
                        return;
                    }

                    CadBlockEntity modelSpace = cadImage.BlockEntities[modelSpaceName];

                    // Add each raster image as a layered background
                    foreach (string rasterPath in rasterPaths)
                    {
                        // Load raster to obtain its dimensions
                        using (Image rasterImg = Image.Load(rasterPath))
                        {
                            int width = rasterImg.Width;
                            int height = rasterImg.Height;

                            // Create raster definition with size parameters
                            CadRasterImageDef rasterDef = new CadRasterImageDef(rasterPath, width, height);

                            // Create raster image object with insertion point at origin
                            CadRasterImage rasterImage = new CadRasterImage(
                                rasterDef,
                                new Cad3DPoint(0, 0, 0),          // insertion point
                                new Cad3DPoint(1, 0, 0),          // U vector (X direction)
                                new Cad3DPoint(0, 1, 0)           // V vector (Y direction)
                            );

                            // Add raster image to the model space entities collection
                            modelSpace.Entities.Add(rasterImage);
                        }
                    }

                    // Save the modified DWG
                    string outputPath = "output.dwg";
                    cadImage.Save(outputPath);
                    Console.WriteLine($"DWG saved with background images to: {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}