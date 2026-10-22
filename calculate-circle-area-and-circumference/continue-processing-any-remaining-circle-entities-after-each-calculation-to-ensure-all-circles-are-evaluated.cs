using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        // Path to the CAD file (DWG/DXF)
        string filePath = "sample.dwg";

        // Ensure the file exists; create a placeholder if missing
        if (!File.Exists(filePath))
        {
            // Create an empty placeholder file to avoid runtime errors
            File.WriteAllBytes(filePath, new byte[0]);
            Console.WriteLine($"Placeholder CAD file created at '{filePath}'. Please replace it with a valid CAD file.");
            return;
        }

        try
        {
            // Load the CAD file
            using (Image image = Image.Load(filePath))
            {
                // Cast to CadImage to work with CAD object model
                CadImage cadImage = (CadImage)image;

                // Process top‑level drawing entities
                ProcessEntities(cadImage.Entities);

                // Process each block definition
                foreach (CadBlockEntity block in cadImage.BlockEntities.Values)
                {
                    ProcessEntities(block.Entities);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }

    // Iterates over a collection of CAD entities and handles circles
    static void ProcessEntities(IEnumerable<CadEntityBase> entities)
    {
        foreach (CadEntityBase entity in entities)
        {
            // Handle circle entities
            if (entity is CadCircle circle)
            {
                Console.WriteLine(
                    $"Circle – Center: ({circle.CenterPoint.X}, {circle.CenterPoint.Y}, {circle.CenterPoint.Z}), Radius: {circle.Radius}");
            }
            // Handle block insert objects if further nested processing is required
            else if (entity is CadInsertObject insert)
            {
                Console.WriteLine($"Insert – Name: {insert.Name}");
            }
        }
    }
}