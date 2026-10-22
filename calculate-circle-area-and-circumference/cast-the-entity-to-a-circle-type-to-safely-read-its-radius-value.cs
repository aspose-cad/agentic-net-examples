using System;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        try
        {
                // Verify that a file path was provided
                if (args.Length == 0)
                {
                    Console.WriteLine("Please provide the path to a DWG/DXF file as the first argument.");
                    return;
                }

                string inputPath = args[0];

        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Load the CAD file
                using (Image image = Image.Load(inputPath))
                {
                    // Cast the generic Image to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Iterate over top‑level drawing entities
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        if (entity is CadCircle circle)
                        {
                            Console.WriteLine($"Circle found: Center=({circle.CenterPoint.X}, {circle.CenterPoint.Y}), Radius={circle.Radius}");
                        }
                    }

                    // Iterate over block definitions and their contained entities
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                            {
                        foreach (CadEntityBase blockEntity in block.Entities)
                        {
                            if (blockEntity is CadCircle blockCircle)
                            {
                                Console.WriteLine($"Circle in block '{block.Name}': Center=({blockCircle.CenterPoint.X}, {blockCircle.CenterPoint.Y}), Radius={blockCircle.Radius}");
                            }
                        }
                    }
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}