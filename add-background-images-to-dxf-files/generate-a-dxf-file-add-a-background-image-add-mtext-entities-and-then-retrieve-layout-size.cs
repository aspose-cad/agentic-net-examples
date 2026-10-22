using System;
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
        try
        {
            // Input DXF file path
            string inputPath = "input.dxf";

            // Ensure the source DXF file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                string minimalDxf = "0\nSECTION\n2\nHEADER\n0\nENDSEC\n0\nSECTION\n2\nENTITIES\n0\nENDSEC\n0\nEOF";
                File.WriteAllText(inputPath, minimalDxf);
            }

            // Load the DXF file
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cadImage = (CadImage)image;

                // ----- Add a background raster image definition -----
                // Placeholder image file name (ensure the file exists when actually running)
                string backgroundImagePath = "background.png";

                // Create raster image definition (width and height in pixels)
                var rasterDef = new CadRasterImageDef(backgroundImagePath, 200, 200);
                // Assign a unique handle (placeholder)
                rasterDef.ObjectHandle = "5";

                // Preserve existing objects and add the new definition
                var existingObjects = cadImage.Objects ?? new CadBaseObject[0];
                cadImage.Objects = existingObjects.Concat(new CadBaseObject[] { rasterDef }).ToArray();

                // ----- Retrieve first layout (if any) -----
                CadLayout layout = null;
                if (cadImage.Layouts != null && cadImage.Layouts.Values != null && cadImage.Layouts.Values.Any())
                {
                    layout = cadImage.Layouts.Values.First();
                }

                // ----- Create the raster image entity -----
                var insertionPoint = new Cad3DPoint { X = 0, Y = 0, Z = 0 };
                var uVector = new Cad3DPoint { X = 200, Y = 0, Z = 0 }; // width direction
                var vVector = new Cad3DPoint { X = 0, Y = 200, Z = 0 }; // height direction

                var rasterImage = new CadRasterImage(rasterDef, insertionPoint, uVector, vVector)
                {
                    ImageDefReference = rasterDef.ObjectHandle,
                    SoftOwner = layout?.BlockTableRecordHandle
                };

                // ----- Add an MTEXT entity -----
                var mtext = new CadMText
                {
                    Text = "Hello Aspose CAD",
                    InsertionPoint = new Cad3DPoint { X = 10, Y = 10, Z = 0 },
                    InitialTextHeight = 5,
                    SoftOwner = layout?.BlockTableRecordHandle
                };

                // Append new entities to the existing collection
                var existingEntities = cadImage.Entities ?? new CadEntityBase[0];
                cadImage.Entities = existingEntities
                    .Concat(new CadEntityBase[] { rasterImage, mtext })
                    .ToArray();

                // ----- Retrieve layout size information -----
                if (layout != null)
                {
                    Console.WriteLine($"Layout Name: {layout.LayoutName}");

                    if (layout.MinExtents != null && layout.MaxExtents != null)
                    {
                        double width = layout.MaxExtents.X - layout.MinExtents.X;
                        double height = layout.MaxExtents.Y - layout.MinExtents.Y;
                        Console.WriteLine($"Layout Width: {width}");
                        Console.WriteLine($"Layout Height: {height}");
                    }
                    else
                    {
                        Console.WriteLine("Layout extents are not defined.");
                    }
                }
                else
                {
                    Console.WriteLine("No layouts found in the DXF file.");
                }

                // Save the modified DXF to a new file
                string outputPath = "output.dxf";
                cadImage.Save(outputPath, new DxfOptions());
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}