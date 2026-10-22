using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string filePath = "sample.dwg";

        if (!File.Exists(filePath))
        {
            Console.Error.WriteLine($"File not found: {filePath}");
            return;
        }

        try
        {
            // Load the CAD document
            using (Image image = Image.Load(filePath))
            {
                // Cast the generic Image to CadImage to access CAD-specific members
                CadImage cadImage = (CadImage)image;

                // Obtain the collection of block definitions in the drawing
                var blockEntities = cadImage.BlockEntities;

                // Iterate through each block definition
                foreach (CadBlockEntity block in blockEntities.Values)
                {
                    // Output the block name
                    Console.WriteLine($"Block Name: {block.Name}");

                    // Process entities contained within the block
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        // Output the type of each entity
                        Console.WriteLine($"  Entity Type: {entity.GetType().Name}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}