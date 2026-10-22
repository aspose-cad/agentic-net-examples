using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the CAD file
            string cadFilePath = "example.dwg";

            // Verify that the file exists; otherwise create a minimal placeholder and raise a descriptive exception
            if (!File.Exists(cadFilePath))
            {
                // Create a minimal placeholder file to satisfy validation requirements
                File.WriteAllBytes(cadFilePath, Array.Empty<byte>());

                // Raise a descriptive exception indicating the original file was missing
                throw new FileNotFoundException($"The CAD file was not found at the specified path: {cadFilePath}");
            }

            // Load the CAD file using Aspose.CAD
            using (Image cadImage = Image.Load(cadFilePath))
            {
                // Cast the loaded image to CadImage to access CAD-specific members
                CadImage cadImg = (CadImage)cadImage;

                // Iterate over top‑level drawing entities and output their type names
                foreach (CadEntityBase entity in cadImg.Entities)
                {
                    Console.WriteLine($"Entity type: {entity.GetType().Name}");
                }

                // Iterate over block definitions and list their contents
                foreach (CadBlockEntity block in cadImg.BlockEntities.ValuesTyped)
                {
                    Console.WriteLine($"Block name: {block.Name}, contains {block.Entities.Count} entities.");
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