using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";

        // Ensure the input file exists; create a minimal placeholder if missing
        if (!File.Exists(inputPath))
        {
            // Create an empty placeholder file to avoid runtime errors
            File.WriteAllBytes(inputPath, new byte[0]);
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Default line weight in DWG/DXF is typically 0 (ByLayer)
                short defaultLineWeight = 0;

                // Iterate over top‑level drawing entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    // Retrieve the line weight of the current entity
                    short entityLineWeight = entity.LineWeight;

                    // Determine whether the entity uses the default line weight
                    bool isDefault = entityLineWeight == defaultLineWeight;

                    // Output entity type and line weight information
                    Console.WriteLine($"{entity.TypeName}: LineWeight = {entityLineWeight} ({(isDefault ? "default" : "custom")})");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}