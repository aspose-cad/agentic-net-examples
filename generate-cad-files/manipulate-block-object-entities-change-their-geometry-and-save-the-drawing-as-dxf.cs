using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file \"{inputPath}\" not found.");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    if (block.Name == "BLOCK1")
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
                            else if (entity is CadLwPolyline polyline)
                            {
                                for (int i = 0; i < polyline.Coordinates.Count; i++)
                                {
                                    Cad2DPoint pt = polyline.Coordinates[i];
                                    Cad2DPoint scaled = new Cad2DPoint(pt.X * 2, pt.Y * 2);
                                    polyline.Coordinates[i] = scaled;
                                }
                            }
                        }
                    }
                }

                try
                {
                    DxfOptions dxfOptions = new DxfOptions();
                    cadImage.Save(outputPath, dxfOptions);
                    Console.WriteLine($"Modified drawing saved to \"{outputPath}\".");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading file: {ex.Message}");
        }
    }
}