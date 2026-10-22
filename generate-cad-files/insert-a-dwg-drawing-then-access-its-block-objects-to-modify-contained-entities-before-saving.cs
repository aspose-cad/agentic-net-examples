using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.dwg";

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
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        if (entity is CadLine line)
                        {
                            Cad3DPoint newFirst = new Cad3DPoint(line.FirstPoint.X + 10, line.FirstPoint.Y, line.FirstPoint.Z);
                            Cad3DPoint newSecond = new Cad3DPoint(line.SecondPoint.X + 10, line.SecondPoint.Y, line.SecondPoint.Z);
                            line.FirstPoint = newFirst;
                            line.SecondPoint = newSecond;
                        }
                    }
                }

                var saveOptions = new DxfOptions();
                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}