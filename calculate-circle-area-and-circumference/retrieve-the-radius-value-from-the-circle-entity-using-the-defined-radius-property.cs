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
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Process top‑level entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadCircle circle)
                    {
                        double radius = circle.Radius;
                        Console.WriteLine($"Circle on layer '{circle.LayerName}' with radius {radius}");
                    }
                }

                // Process block definitions
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Block: {block.Name}");
                    foreach (CadEntityBase blockEntity in block.Entities)
                    {
                        if (blockEntity is CadCircle blockCircle)
                        {
                            double radius = blockCircle.Radius;
                            Console.WriteLine($"  Circle in block '{block.Name}' on layer '{blockCircle.LayerName}' with radius {radius}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}