using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Block name: {block.Name}");

                    foreach (CadEntityBase entity in block.Entities)
                    {
                        if (entity.TypeName == CadEntityTypeName.LWPOLYLINE)
                        {
                            CadLwPolyline lwPolyline = (CadLwPolyline)entity;
                            Console.WriteLine($"  LWPOLYLINE with {lwPolyline.PointCount} points.");

                            if (lwPolyline.ConstantWidth == 0)
                            {
                                lwPolyline.ConstantWidth = 0.5;
                                Console.WriteLine("    Constant width set to 0.5");
                            }
                        }
                    }
                }

                DxfOptions saveOptions = new DxfOptions { OutputMode = CadOutputMode.Convert };
                cadImage.Save(outputPath);
                Console.WriteLine($"Saved modified drawing to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}