using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
                // Path to the source CAD file
                string inputPath = "input.dwg";

        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Load the CAD file using Aspose.CAD.Image.Load
                using (Image image = Image.Load(inputPath))
                {
                    // Cast the loaded image to CadImage to work with CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Iterate through all block definitions in the drawing
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        // Iterate through each entity inside the current block
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            // Check if the entity is a text entity
                            if (entity is CadText textEntity)
                            {
                                // Modify the text content of the entity
                                textEntity.DefaultValue = "Modified Text";
                            }
                        }
                    }

                    // Save the modified drawing to a new file
                    string outputPath = "output.dwg";
                    // For DWG output, the default save behavior is sufficient
                    cadImage.Save(outputPath);
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}