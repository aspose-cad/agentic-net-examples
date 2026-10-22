using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.dwg";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        CadImage cadImage = null;
        try
        {
            cadImage = (CadImage)Image.Load(inputPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load CAD file: {ex.Message}");
            return;
        }

        using (cadImage)
        {
            try
            {
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        if (entity is CadLine line)
                        {
                            var p1 = line.FirstPoint;
                            var p2 = line.SecondPoint;
                            line.FirstPoint = new Cad3DPoint(p1.X + 10, p1.Y, p1.Z);
                            line.SecondPoint = new Cad3DPoint(p2.X + 10, p2.Y, p2.Z);
                        }
                        else if (entity is CadLwPolyline polyline)
                        {
                            for (int i = 0; i < polyline.Coordinates.Count; i++)
                            {
                                var pt = polyline.Coordinates[i];
                                polyline.Coordinates[i] = new Cad2DPoint(pt.X + 10, pt.Y);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing block entities: {ex.Message}");
                return;
            }

            try
            {
                cadImage.Save(outputPath);
                Console.WriteLine($"File saved successfully to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save CAD file: {ex.Message}");
            }
        }
    }
}