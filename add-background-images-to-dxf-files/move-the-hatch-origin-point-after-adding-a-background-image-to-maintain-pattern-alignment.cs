using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dxf";
        const string backgroundPath = "background.png";
        const string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        if (!File.Exists(backgroundPath))
        {
            Console.WriteLine($"Background image not found: {backgroundPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Create raster image definition
                var rasterDef = new CadRasterImageDef
                {
                    FileName = backgroundPath,
                    ObjectHandle = "IMG1"
                };

                // Preserve existing objects and add the new raster definition
                var objectsList = new List<CadBaseObject>(cadImage.Objects ?? Array.Empty<CadBaseObject>());
                objectsList.Add(rasterDef);
                cadImage.Objects = objectsList.ToArray();

                // Define insertion point and size vectors for the background image
                var insertionPoint = new Cad3DPoint(0, 0, 0);
                double width = 200;
                double height = 200;
                var uVector = new Cad3DPoint(width, 0, 0);
                var vVector = new Cad3DPoint(0, height, 0);

                // Create raster image entity referencing the definition
                var backgroundRaster = new CadRasterImage(rasterDef, insertionPoint, uVector, vVector)
                {
                    ImageDefReference = rasterDef.ObjectHandle,
                    ImageSizeU = width,
                    ImageSizeV = height
                };

                // Add the raster image entity to the drawing's entities collection
                var entitiesList = new List<CadEntityBase>(cadImage.Entities ?? Array.Empty<CadEntityBase>());
                entitiesList.Add(backgroundRaster);
                cadImage.Entities = entitiesList.ToArray();

                // Save the modified drawing to PDF
                var pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Output saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
