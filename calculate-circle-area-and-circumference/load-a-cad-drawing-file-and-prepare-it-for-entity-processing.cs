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
        // Placeholder input CAD file path
        string inputPath = "input.dwg";

        // Verify that the input file exists
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Prepare for entity processing: iterate over top-level entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    // Example processing: output entity type and layer name
                    string typeName = entity.GetType().Name;
                    string layerName = entity.LayerName ?? "Default";
                    Console.WriteLine($"Entity Type: {typeName}, Layer: {layerName}");
                }

                // Example processing of block definitions
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Block Name: {block.Name}, Entities Count: {block.Entities.Count}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}