using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

public class Program
{
    public static void Main(string[] args)
    {
        string inputPath = "input.dwg";
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image cadImage = Image.Load(inputPath))
            {
                CadImage cad = cadImage as CadImage;
                if (cad == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                List<string> xrefPathNames = new List<string>();

                foreach (CadEntityBase entity in cad.Entities)
                {
                    CadRasterImage rasterImage = entity as CadRasterImage;
                    if (rasterImage != null)
                    {
                        string imageDefHandle = rasterImage.ImageDefReference;
                        foreach (CadBaseObject obj in cad.Objects)
                        {
                            CadRasterImageDef rasterDef = obj as CadRasterImageDef;
                            if (rasterDef != null && rasterDef.ObjectHandle == imageDefHandle)
                            {
                                string pathName = rasterDef.FileName;
                                xrefPathNames.Add(pathName);
                                break;
                            }
                        }
                    }
                }

                foreach (string pathName in xrefPathNames)
                {
                    Console.WriteLine(pathName);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}