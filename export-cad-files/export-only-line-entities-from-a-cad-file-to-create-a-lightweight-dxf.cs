using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string sourcePath = "source.dwg";
        const string outputPath = "lines_only.dxf";

        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file '{sourcePath}' does not exist.");
            return;
        }

        try
        {
            using (Image sourceImage = Image.Load(sourcePath))
            {
                CadImage sourceCad = (CadImage)sourceImage;

                var lineEntities = new List<CadEntityBase>();

                foreach (CadEntityBase entity in sourceCad.Entities)
                {
                    if (entity is CadLine line)
                    {
                        lineEntities.Add(line);
                    }
                }

                CadImage outputCad = new CadImage();
                outputCad.Entities = lineEntities.ToArray();

                DxfOptions dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };

                outputCad.Save(outputPath, dxfOptions);
            }

            Console.WriteLine($"Lines-only DXF saved to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}