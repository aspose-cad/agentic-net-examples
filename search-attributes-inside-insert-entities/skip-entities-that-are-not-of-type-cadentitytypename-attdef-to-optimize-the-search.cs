using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the DWG/DXF file
            string cadFilePath = "input.dwg";

            // Verify the file exists before attempting to load it
            if (!File.Exists(cadFilePath))
            {
                Console.WriteLine($"File not found: {cadFilePath}");
                return;
            }

            // Load the CAD file
            using (Image image = Image.Load(cadFilePath))
            {
                if (image is not CadImage cadImage)
                {
                    Console.WriteLine("Unable to load CAD image.");
                    return;
                }

                // Iterate over top‑level drawing entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    // Skip entities that are not ATTDEF
                    if (entity.TypeName != CadEntityTypeName.ATTDEF)
                        continue;

                    // Process ATTDEF entity
                    Console.WriteLine($"Found ATTDEF entity. Type: {entity.TypeName}");
                    Console.WriteLine($"Layer: {entity.LayerName}");
                    Console.WriteLine($"Entity CLR type: {entity.GetType().Name}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}