using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"File not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;
                double totalLength = 0.0;

                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        if (entity is CadLine line)
                        {
                            double dx = line.SecondPoint.X - line.FirstPoint.X;
                            double dy = line.SecondPoint.Y - line.FirstPoint.Y;
                            double dz = line.SecondPoint.Z - line.FirstPoint.Z;
                            double length = Math.Sqrt(dx * dx + dy * dy + dz * dz);
                            totalLength += length;
                        }
                    }
                }

                Console.WriteLine($"Total length of all line objects inside blocks: {totalLength}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}