using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using Aspose.CAD.ImageOptions;

namespace AddBackgroundImageToDxf
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input DXF file path
                string dxfPath = "input.dxf";
                // Raster image to be added as background
                string rasterPath = "background.png";
                // Output DXF file path
                string outputPath = "output.dxf";

                // Verify that required files exist
                if (!File.Exists(dxfPath))
                {
                    Console.WriteLine($"DXF file not found: {dxfPath}");
                    return;
                }

                if (!File.Exists(rasterPath))
                {
                    Console.WriteLine($"Raster image file not found: {rasterPath}");
                    return;
                }

                // Load the raster image to obtain its dimensions
                double imageWidth;
                double imageHeight;
                using (Image rasterImg = Image.Load(rasterPath))
                {
                    imageWidth = rasterImg.Width;
                    imageHeight = rasterImg.Height;
                }

                // Load the DXF drawing
                using (Image img = Image.Load(dxfPath))
                {
                    CadImage cadImage = (CadImage)img;

                    // Retrieve the Model Space block where drawing entities reside
                    if (!(cadImage.BlockEntities["*Model_Space"] is CadBlockEntity modelSpace))
                    {
                        Console.WriteLine("Model space block not found in the DXF file.");
                        return;
                    }

                    // -------------------------------------------------
                    // 1. Create a raster image definition and add it to the drawing objects
                    // -------------------------------------------------
                    var rasterDef = new CadRasterImageDef
                    {
                        FileName = rasterPath,
                        ObjectHandle = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpperInvariant(),
                        ImageSizeU = imageWidth,
                        ImageSizeV = imageHeight
                    };

                    var objectsList = cadImage.Objects?.ToList() ?? new List<CadBaseObject>();
                    objectsList.Add(rasterDef);
                    cadImage.Objects = objectsList.ToArray();

                    // -------------------------------------------------
                    // 2. Create a raster image entity that references the definition
                    // -------------------------------------------------
                    var insertionPoint = new Cad3DPoint(0, 0, 0);
                    var uVector = new Cad3DPoint(imageWidth, 0, 0);   // width direction
                    var vVector = new Cad3DPoint(0, imageHeight, 0);  // height direction

                    var rasterImage = new CadRasterImage(rasterDef, insertionPoint, uVector, vVector);

                    // Add the raster image to the model space
                    modelSpace.AddEntity(rasterImage);

                    // -------------------------------------------------
                    // 3. Offset existing drawing entities down to make room for a title
                    // -------------------------------------------------
                    double yOffset = 50.0;

                    foreach (var blkObj in cadImage.BlockEntities.Values)
                    {
                        if (blkObj is CadBlockEntity blk)
                        {
                            for (int i = 0; i < blk.Entities.Count; i++)
                            {
                                var entity = blk.Entities.ElementAt(i);

                                switch (entity)
                                {
                                    case CadLine line:
                                        var p1 = line.FirstPoint;
                                        var p2 = line.SecondPoint;
                                        line.FirstPoint = new Cad3DPoint(p1.X, p1.Y + yOffset, p1.Z);
                                        line.SecondPoint = new Cad3DPoint(p2.X, p2.Y + yOffset, p2.Z);
                                        break;

                                    case CadLwPolyline polyline:
                                        for (int j = 0; j < polyline.Coordinates.Count; j++)
                                        {
                                            var pt = polyline.Coordinates[j];
                                            polyline.Coordinates[j] = new Cad2DPoint(pt.X, pt.Y + yOffset);
                                        }
                                        break;

                                    case CadPolyline poly3d:
                                        foreach (var vertex in poly3d.ChildObjects.OfType<CadVertexBase>())
                                        {
                                            var loc = vertex.LocationPoint;
                                            vertex.LocationPoint = new Cad3DPoint(loc.X, loc.Y + yOffset, loc.Z);
                                        }
                                        break;

                                    // Add handling for other entity types as needed
                                }
                            }
                        }
                    }

                    // -------------------------------------------------
                    // 4. Add a simple text title at the top of the drawing
                    // -------------------------------------------------
                    var titleText = new CadText
                    {
                        DefaultValue = "Drawing Title",
                        FirstAlignment = new Cad3DPoint(0, yOffset / 2, 0),
                        TextHeight = 10,
                        SoftOwner = cadImage.Layouts["Model"].BlockTableRecordHandle
                    };
                    modelSpace.AddEntity(titleText);

                    // -------------------------------------------------
                    // 5. Save the modified drawing
                    // -------------------------------------------------
                    cadImage.Save(outputPath, new DxfOptions());
                    Console.WriteLine($"Modified DXF saved to: {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}