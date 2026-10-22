using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace AddBackgroundImageExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input DWG file and raster image to be added as background
                string dwgPath = "input.dwg";
                string rasterPath = "background.png";
                string outputPath = "output.dwg";

                // Verify that both source files exist
                if (!File.Exists(dwgPath))
                {
                    Console.WriteLine($"DWG file not found: {dwgPath}");
                    return;
                }

                if (!File.Exists(rasterPath))
                {
                    Console.WriteLine($"Raster image file not found: {rasterPath}");
                    return;
                }

                // Load the DWG file
                using (Image img = Image.Load(dwgPath))
                {
                    CadImage cadImage = img as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("Failed to load CAD image.");
                        return;
                    }

                    // Load raster image to obtain its dimensions
                    using (Image rasterImg = Image.Load(rasterPath))
                    {
                        double imgWidth = rasterImg.Width;
                        double imgHeight = rasterImg.Height;

                        // Create raster image definition and add it to the Objects collection
                        CadRasterImageDef rasterDef = new CadRasterImageDef(rasterPath, (int)imgWidth, (int)imgHeight);
                        var objects = cadImage.Objects?.ToList() ?? new List<CadBaseObject>();
                        objects.Add(rasterDef);
                        cadImage.Objects = objects.ToArray();

                        // Define insertion point and orientation vectors
                        Cad3DPoint insertionPoint = new Cad3DPoint(0, 0, 0);
                        Cad3DPoint uVector = new Cad3DPoint(imgWidth, 0, 0);
                        Cad3DPoint vVector = new Cad3DPoint(0, imgHeight, 0);

                        // Create raster image entity and link it to its definition
                        CadRasterImage rasterImage = new CadRasterImage(rasterDef, insertionPoint, uVector, vVector)
                        {
                            ImageDefReference = rasterDef.ObjectHandle
                        };

                        // Add the raster image entity to the drawing's main entity collection
                        var entities = cadImage.Entities?.ToList() ?? new List<CadEntityBase>();
                        entities.Add(rasterImage);
                        cadImage.Entities = entities.ToArray();

                        // Also add the entity to Model Space block if available
                        if (cadImage.BlockEntities.TryGetValue("*Model_Space", out var modelSpaceBlock))
                        {
                            if (modelSpaceBlock is CadBlockEntity modelSpace)
                            {
                                var blockEntities = modelSpace.Entities?.ToList() ?? new List<CadEntityBase>();
                                blockEntities.Add(rasterImage);
                                modelSpace.Entities = blockEntities.ToArray();

                                Console.WriteLine("Entities in Model Space after addition:");
                                foreach (var ent in modelSpace.Entities)
                                {
                                    Console.WriteLine($" - {ent.GetType().Name}");
                                }
                            }
                        }

                        // Save the modified drawing
                        cadImage.Save(outputPath);
                        Console.WriteLine($"Background image added and saved to {outputPath}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}