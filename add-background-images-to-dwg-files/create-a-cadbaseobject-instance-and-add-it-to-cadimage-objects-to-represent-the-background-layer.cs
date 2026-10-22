using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace AddBackgroundToDwg
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Paths to the source DWG file and the output file
                string inputPath = "input.dwg";
                string outputPath = "output.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the DWG file
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Create a CadRasterImageDef as a CadBaseObject to act as a background definition
                    var backgroundDef = new CadRasterImageDef
                    {
                        // Placeholder values – replace with actual background image details
                        FileName = "background.png",          // Path to background image file
                        ObjectHandle = "*D1",                // Unique handle for the definition
                        ImageSizeU = 200.0,                  // Width of the image in drawing units
                        ImageSizeV = 150.0                   // Height of the image in drawing units
                    };

                    // Preserve existing objects and add the new background definition
                    cadImage.Objects = cadImage.Objects
                        .Concat(new CadBaseObject[] { backgroundDef })
                        .ToArray();

                    // Optionally, add a CadRasterImage entity that references the definition
                    var backgroundImage = new CadRasterImage
                    {
                        ImageDefReference = backgroundDef.ObjectHandle,
                        InsertionPoint = new Cad3DPoint(0, 0, 0),
                        UVector = new Cad3DPoint(backgroundDef.ImageSizeU, 0, 0),
                        VVector = new Cad3DPoint(0, backgroundDef.ImageSizeV, 0),
                        ImageSizeU = backgroundDef.ImageSizeU,
                        ImageSizeV = backgroundDef.ImageSizeV
                    };

                    // Add the raster image entity to the drawing entities
                    cadImage.Entities = cadImage.Entities
                        .Concat(new CadEntityBase[] { backgroundImage })
                        .ToArray();

                    // Save the modified DWG file
                    cadImage.Save(outputPath, new DwgOptions());
                }

                Console.WriteLine($"Background added and file saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}