using System;
using System.Collections.Generic;
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
        string inputPath = "input.dxf";
        string backgroundPath = "background.png";
        string outputPath = "output.pdf";

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
            using (Image loadedImage = Image.Load(inputPath))
            {
                CadImage dxfImage = loadedImage as CadImage;
                if (dxfImage == null)
                {
                    Console.WriteLine("The loaded file is not a DXF/CAD image.");
                    return;
                }

                // Create raster image definition for the background image
                var rasterDef = new CadRasterImageDef
                {
                    FileName = backgroundPath,
                    ObjectHandle = "IMGDEF1"
                };

                // Add the definition to the Objects collection, preserving existing objects
                var objects = dxfImage.Objects?.ToList() ?? new List<CadBaseObject>();
                objects.Add(rasterDef);
                dxfImage.Objects = objects.ToArray();

                // Create a raster image entity that references the definition
                var insertionPoint = new Cad3DPoint(0, 0, 0);
                var uVector = new Cad3DPoint(1, 0, 0); // X direction scale
                var vVector = new Cad3DPoint(0, 1, 0); // Y direction scale

                var rasterImage = new CadRasterImage(rasterDef, insertionPoint, uVector, vVector)
                {
                    ImageDefReference = rasterDef.ObjectHandle
                };

                // Add the raster image entity to the drawing, preserving existing entities
                var entities = dxfImage.Entities?.ToList() ?? new List<CadEntityBase>();
                entities.Add(rasterImage);
                dxfImage.Entities = entities.ToArray();

                // Prepare rasterization options for PDF export
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new[] { "Model" },
                    UnitType = UnitType.Centimenter,
                    LineScale = 1.0f
                };

                // Set PDF options
                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the modified DXF as PDF
                dxfImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"PDF saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
