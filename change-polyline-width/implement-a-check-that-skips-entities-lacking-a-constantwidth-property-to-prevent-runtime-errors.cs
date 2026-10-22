using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        const string filePath = "sample.dwg";

        // Ensure the input file exists; create a minimal placeholder if it does not.
        if (!File.Exists(filePath))
        {
            // Create an empty placeholder file to avoid load errors.
            // In a real scenario, this should be a valid CAD file.
            File.WriteAllBytes(filePath, new byte[0]);
            Console.WriteLine($"Placeholder CAD file created at '{filePath}'.");
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(filePath))
            {
                CadImage cadImage = (CadImage)image;

                // Process top‑level drawing entities
                Console.WriteLine("Top‑level entities with ConstantWidth:");
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    ProcessEntity(entity);
                }

                // Process block definitions and their inner entities
                Console.WriteLine("\nBlock entities with ConstantWidth:");
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Block: {block.Name}");
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        ProcessEntity(entity);
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

    static void ProcessEntity(CadEntityBase entity)
    {
        // Check whether the entity type defines a ConstantWidth property
        PropertyInfo propInfo = entity.GetType().GetProperty("ConstantWidth", BindingFlags.Public | BindingFlags.Instance);
        if (propInfo != null)
        {
            object widthValue = propInfo.GetValue(entity);
            Console.WriteLine($"{entity.TypeName}: ConstantWidth = {widthValue}");
        }
        // Entities without ConstantWidth are skipped to avoid runtime errors
    }
}