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
        try
        {
                // Path to the CAD file
                string inputPath = "sample.dxf";

                // Verify that the file exists before attempting to load
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"File not found: {inputPath}");
                    return;
                }

                // Load the CAD file and cast to CadImage to access CAD object model
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // Iterate over block definitions in the drawing
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        Console.WriteLine($"Block name: {block.Name}");

                        // Iterate over entities contained within the block
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            switch (entity)
                            {
                                case CadCircle circle when circle is not CadArc:
                                    Console.WriteLine($"Circle - Center: ({circle.CenterPoint.X}, {circle.CenterPoint.Y}), Radius: {circle.Radius}");
                                    break;

                                case CadArc arc:
                                    Console.WriteLine($"Arc - Center: ({arc.CenterPoint.X}, {arc.CenterPoint.Y}), Radius: {arc.Radius}, StartAngle: {arc.StartAngle}, EndAngle: {arc.EndAngle}");
                                    break;

                                case CadLine line:
                                    Console.WriteLine($"Line - Start: ({line.FirstPoint.X}, {line.FirstPoint.Y}), End: ({line.SecondPoint.X}, {line.SecondPoint.Y})");
                                    break;

                                default:
                                    Console.WriteLine($"Entity type: {entity.GetType().Name}");
                                    break;
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