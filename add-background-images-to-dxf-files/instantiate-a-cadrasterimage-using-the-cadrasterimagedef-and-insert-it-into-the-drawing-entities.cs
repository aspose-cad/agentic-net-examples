using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace AddBackgroundImageExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Paths to the source DXF, the raster image to insert, and the output DXF.
                string sourceDxfPath = "input.dxf";
                string rasterImagePath = "background.png";
                string outputDxfPath = "output.dxf";

                // Verify that input files exist.
                if (!File.Exists(sourceDxfPath))
                {
                    Console.Error.WriteLine($"Source DXF file not found: {sourceDxfPath}");
                    return;
                }
                if (!File.Exists(rasterImagePath))
                {
                    Console.Error.WriteLine($"Raster image file not found: {rasterImagePath}");
                    return;
                }

                // Load the DXF file as a CadImage.
                using (CadImage cadImage = (CadImage)Image.Load(sourceDxfPath))
                {
                    // Create a raster image definition that points to the external raster file.
                    // Width and height are placeholder values; adjust as needed.
                    var rasterDef = new CadRasterImageDef(rasterImagePath, 200, 150);

                    // Add the raster definition to the image's objects collection.
                    var objects = cadImage.Objects?.ToList() ?? new List<CadBaseObject>();
                    objects.Add(rasterDef);
                    cadImage.Objects = objects.ToArray();

                    // Create a raster image entity that references the definition.
                    var insertionPoint = new Cad3DPoint(0, 0, 0);
                    var uVector = new Cad3DPoint(200, 0, 0); // Width direction
                    var vVector = new Cad3DPoint(0, 150, 0); // Height direction
                    var rasterEntity = new CadRasterImage(rasterDef, insertionPoint, uVector, vVector);

                    // Insert the raster image entity into the drawing's top‑level entities collection.
                    var entities = cadImage.Entities?.ToList() ?? new List<CadEntityBase>();
                    entities.Add(rasterEntity);
                    cadImage.Entities = entities.ToArray();

                    // Save the modified drawing as DXF.
                    var dxfOptions = new DxfOptions();
                    cadImage.Save(outputDxfPath, dxfOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}