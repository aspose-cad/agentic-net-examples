using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
                const string cadFilePath = "sample.dwg";

                if (!File.Exists(cadFilePath))
                {
                    Console.WriteLine($"File not found: {cadFilePath}");
                    return;
                }

                // Load the CAD file
                using (CadImage cadImage = (CadImage)Image.Load(cadFilePath))
                {
                    // Process top‑level entities
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        if (entity.TypeName == CadEntityTypeName.CIRCLE)
                        {
                            CadCircle circle = (CadCircle)entity;
                            double radius = circle.Radius;
                            double area = Math.PI * radius * radius;
                            double circumference = 2 * Math.PI * radius;

                            Console.WriteLine($"Circle at ({circle.CenterPoint.X}, {circle.CenterPoint.Y}) Radius: {radius}");
                            Console.WriteLine($"Area: {area}, Circumference: {circumference}");
                        }
                    }

                    // Process block definitions and their contained entities
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        if (block == null) continue;

                        Console.WriteLine($"Block: {block.Name}");

                        foreach (CadEntityBase blockEntity in block.Entities)
                        {
                            if (blockEntity.TypeName == CadEntityTypeName.CIRCLE)
                            {
                                CadCircle circle = (CadCircle)blockEntity;
                                double radius = circle.Radius;
                                double area = Math.PI * radius * radius;
                                double circumference = 2 * Math.PI * radius;

                                Console.WriteLine($"  Circle in block at ({circle.CenterPoint.X}, {circle.CenterPoint.Y}) Radius: {radius}");
                                Console.WriteLine($"  Area: {area}, Circumference: {circumference}");
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