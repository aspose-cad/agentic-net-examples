using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace AddBackgroundImageToDxf
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input DXF file path
                string inputPath = "input.dxf";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Background image file path (any raster format supported by Aspose.CAD)
                string imagePath = "background.png";

                if (!File.Exists(imagePath))
                {
                    Console.WriteLine($"Image file not found: {imagePath}");
                    return;
                }

                // Output DXF file path
                string outputPath = "output/output.dxf";

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the DXF drawing
                using (Image img = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)img;

                    // Placeholder image size (width and height in drawing units)
                    double imageWidth = 200.0;
                    double imageHeight = 150.0;

                    // Create raster image definition (using file name and placeholder size)
                    CadRasterImageDef rasterDef = new CadRasterImageDef(imagePath, (int)imageWidth, (int)imageHeight);

                    // Add the raster definition to the drawing's objects collection
                    var objectsList = cadImage.Objects?.ToList() ?? new List<CadBaseObject>();
                    objectsList.Add(rasterDef);
                    cadImage.Objects = objectsList.ToArray();

                    // Create a raster image entity that references the definition
                    CadRasterImage rasterImage = new CadRasterImage(
                        rasterDef,
                        new Cad3DPoint(0.0, 0.0, 0.0),                 // Insertion point
                        new Cad3DPoint(imageWidth, 0.0, 0.0),          // U vector (width direction)
                        new Cad3DPoint(0.0, imageHeight, 0.0)         // V vector (height direction)
                    );

                    // Insert the raster image into the drawing's top-level entities
                    cadImage.Entities = cadImage.Entities
                        .Concat(new CadEntityBase[] { rasterImage })
                        .ToArray();

                    // Add a TEXT entity on top of the image
                    CadText textEntity = new CadText
                    {
                        DefaultValue = "Sample Text",
                        FirstAlignment = new Cad3DPoint(10.0, 10.0, 0.0),
                        TextHeight = 5.0,
                        TextRotation = 0.0,
                        LayerName = "0"
                    };

                    cadImage.Entities = cadImage.Entities
                        .Concat(new CadEntityBase[] { textEntity })
                        .ToArray();

                    // Save the modified drawing as DXF
                    cadImage.Save(outputPath);
                }

                Console.WriteLine($"DXF saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}