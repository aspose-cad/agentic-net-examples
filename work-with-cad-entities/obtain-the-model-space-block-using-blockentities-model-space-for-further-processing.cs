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
            // Path to the CAD file
            string filePath = "sample.dwg";

            // Guard against missing file
            if (!File.Exists(filePath))
            {
                Console.Error.WriteLine($"File not found: {filePath}");
                return;
            }

            // Load the CAD image
            using (Image image = Image.Load(filePath))
            {
                // Cast to CadImage to work with CAD object model
                CadImage cadImage = (CadImage)image;

                // Obtain the Model Space block
                if (!cadImage.BlockEntities.TryGetValue("*Model_Space", out CadBlockEntity modelSpaceBlock) || modelSpaceBlock == null)
                {
                    Console.Error.WriteLine("Model Space block not found.");
                    return;
                }

                // Process entities inside Model Space
                foreach (CadEntityBase entity in modelSpaceBlock.Entities)
                {
                    // Output the type name of each entity
                    Console.WriteLine($"Entity Type: {entity.TypeName}");
                    // Additional processing can be added here
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}