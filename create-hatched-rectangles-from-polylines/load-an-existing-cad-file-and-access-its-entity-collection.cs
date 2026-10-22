using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        // Path to the CAD file (replace with actual path)
        string sourceFile = "sample.dwg";

        // Verify that the file exists before attempting to load it
        if (!File.Exists(sourceFile))
        {
            Console.Error.WriteLine($"File not found: {sourceFile}");
            return;
        }

        try
        {
            // Load the CAD file
            using (Image image = Image.Load(sourceFile))
            {
                // Cast to CadImage to work with CAD-specific API
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Unable to cast loaded image to CadImage.");
                    return;
                }

                // Access the collection of entities
                foreach (CadEntityBase cadEntity in cadImage.Entities)
                {
                    // Print the entity type name
                    Console.WriteLine("Entity Type: " + cadEntity.TypeName);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}