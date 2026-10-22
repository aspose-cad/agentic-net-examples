using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "sample.dxf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            CadImage cad = (CadImage)Image.Load(inputPath);

            // Collection to hold external raster image definitions (IMAGEDEF)
            List<CadRasterImageDef> imageDefs = new List<CadRasterImageDef>();

            // Iterate through all objects stored in the drawing
            if (cad.Objects != null)
            {
                foreach (var obj in cad.Objects)
                {
                    if (obj is CadRasterImageDef rasterDef)
                    {
                        imageDefs.Add(rasterDef);
                    }
                }
            }

            // Enumerate and display information about each external image reference
            Console.WriteLine("External IMAGEDEF references found:");
            foreach (var def in imageDefs)
            {
                Console.WriteLine($"Handle: {def.ObjectHandle}");
                Console.WriteLine($"File Name: {def.FileName}");
                Console.WriteLine($"Image Size U: {def.ImageSizeU}");
                Console.WriteLine($"Image Size V: {def.ImageSizeV}");
                Console.WriteLine(new string('-', 40));
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}