using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Loaded file is not a CAD image.");
                    return;
                }

                // Process top‑level entities
                ProcessEntities(cadImage.Entities);

                // Process entities inside each block definition
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    ProcessEntities(block.Entities);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void ProcessEntities(System.Collections.Generic.IEnumerable<CadEntityBase> entities)
    {
        foreach (CadEntityBase entity in entities)
        {
            if (entity is CadCircle circle)
            {
                double radius = circle.Radius;
                double area = Math.PI * radius * radius;
                double circumference = 2 * Math.PI * radius;
                string layerName = circle.LayerName ?? "0";

                Console.WriteLine($"Circle on layer '{layerName}': Center=({circle.CenterPoint.X}, {circle.CenterPoint.Y}, {circle.CenterPoint.Z}), Radius={radius:F3}");
                Console.WriteLine($"  Area = {area:F3}");
                Console.WriteLine($"  Circumference = {circumference:F3}");
            }
        }
    }
}