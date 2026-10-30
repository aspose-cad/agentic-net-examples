using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace AddBackgroundImagesExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                string sourceCadPath = "input.dwg";
                string[] backgroundImagePaths = { "bg1.png", "bg2.png", "bg3.png" };
                string outputPath = "output.dwg";

                // Verify source CAD file exists
                if (!File.Exists(sourceCadPath))
                {
                    Console.WriteLine($"Source CAD file not found: {sourceCadPath}");
                    return;
                }

                // Verify background image files exist
                foreach (var imgPath in backgroundImagePaths)
                {
                    if (!File.Exists(imgPath))
                    {
                        Console.WriteLine($"Background image file not found: {imgPath}");
                        return;
                    }
                }

                // Ensure output directory exists
                string? outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the CAD drawing
                using (CadImage cadImage = (CadImage)Image.Load(sourceCadPath))
                {
                    // Preserve existing objects
                    var objectsList = cadImage.Objects?.ToList() ?? new List<CadBaseObject>();

                    foreach (var imgPath in backgroundImagePaths)
                    {
                        // Load image to obtain dimensions
                        using (Image img = Image.Load(imgPath))
                        {
                            double width = img.Width;
                            double height = img.Height;

                            // Create raster image definition
                            var rasterDef = new CadRasterImageDef
                            {
                                FileName = imgPath,
                                ImageSizeU = width,
                                ImageSizeV = height,
                                ObjectHandle = Guid.NewGuid().ToString("X")
                            };
                            objectsList.Add(rasterDef);

                            // Create block table record
                            var blockTable = new CadBlockTableObject
                            {
                                BlockName = Path.GetFileNameWithoutExtension(imgPath)
                            };

                            // Create block entity and associate table record
                            var blockEntity = new CadBlockEntity
                            {
                                Name = blockTable.BlockName,
                                CadBlockTableObject = blockTable
                            };

                            // Create raster image entity referencing the definition
                            var insertionPoint = new Cad3DPoint(0, 0, 0);
                            var uVector = new Cad3DPoint(width, 0, 0);
                            var vVector = new Cad3DPoint(0, height, 0);
                            var rasterImage = new CadRasterImage(rasterDef, insertionPoint, uVector, vVector);

                            // Add raster image to block entity
                            blockEntity.AddEntity(rasterImage);

                            // Add block entity to the drawing
                            cadImage.BlockEntities[blockEntity.Name] = blockEntity;
                        }
                    }

                    // Update the Objects array with new definitions
                    cadImage.Objects = objectsList.ToArray();

                    // Save the modified drawing
                    var saveOptions = new DwgOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };
                    cadImage.Save(outputPath);
                }

                Console.WriteLine("Background images processed and file saved successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}