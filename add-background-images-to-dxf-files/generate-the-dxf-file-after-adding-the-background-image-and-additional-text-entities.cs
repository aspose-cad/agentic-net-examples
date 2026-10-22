using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for input DXF, background image, and output DXF
            string inputDxfPath = "input.dxf";
            string backgroundImagePath = "background.png";
            string outputDxfPath = "output.dxf";

            // Verify that source files exist
            if (!File.Exists(inputDxfPath))
            {
                Console.WriteLine($"Input DXF file not found: {inputDxfPath}");
                return;
            }
            if (!File.Exists(backgroundImagePath))
            {
                Console.WriteLine($"Background image file not found: {backgroundImagePath}");
                return;
            }

            // Load the DXF file
            using (Image loadedImage = Image.Load(inputDxfPath))
            {
                CadImage cadImage = loadedImage as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // ----- Create raster image definition -----
                var rasterDef = new CadRasterImageDef
                {
                    FileName = backgroundImagePath,
                    ObjectHandle = "DEF1"
                };

                // Add definition to the Objects collection, preserving existing objects
                cadImage.Objects = cadImage.Objects.Concat(new CadBaseObject[] { rasterDef }).ToArray();

                // Define insertion point and size (example: 200x200 units)
                double width = 200.0;
                double height = 200.0;
                var insertionPoint = new Cad3DPoint(0, 0, 0);
                var uVector = new Cad3DPoint(width, 0, 0);
                var vVector = new Cad3DPoint(0, height, 0);

                // ----- Create raster image entity -----
                var rasterImage = new CadRasterImage(rasterDef, insertionPoint, uVector, vVector)
                {
                    LayerName = "Background"
                };

                // Add raster image entity to the drawing
                cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { rasterImage }).ToArray();

                // ----- Add a text entity -----
                var textEntity = new CadText
                {
                    DefaultValue = "Sample Text",
                    FirstAlignment = new Cad3DPoint(10, 10, 0),
                    TextHeight = 5.0,
                    LayerName = "Annotations"
                };

                // Add the text entity to the drawing
                cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { textEntity }).ToArray();

                // Save the modified drawing as DXF
                var dxfSaveOptions = new DxfOptions();
                cadImage.Save(outputDxfPath, dxfSaveOptions);
            }

            Console.WriteLine("DXF file saved with background image and text entities.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}