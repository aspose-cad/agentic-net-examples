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
            // Path to the DWG file to be loaded
            string sourceFile = "input.dwg";

            // Guard against missing file
            if (!File.Exists(sourceFile))
            {
                Console.Error.WriteLine($"File not found: {sourceFile}");
                return;
            }

            // Load the DWG file using Aspose.CAD.Image.Load
            using (Image image = Image.Load(sourceFile))
            {
                // Cast the loaded image to CadImage to access CAD-specific members
                CadImage cadImage = (CadImage)image;

                // Iterate through top-level drawing entities in the DWG file
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    // Output the entity type name
                    Console.WriteLine("Entity Type: " + entity.GetType().Name);
                    // Additional entity-specific processing can be performed here
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}