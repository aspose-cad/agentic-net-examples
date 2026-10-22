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
        const string inputPath = "input.dwg";

        // Ensure the input file exists; create a minimal placeholder if it does not.
        if (!File.Exists(inputPath))
        {
            // Create an empty placeholder file to avoid runtime errors.
            // In a real scenario, this should be a valid CAD file.
            File.WriteAllBytes(inputPath, Array.Empty<byte>());
            Console.WriteLine($"Placeholder CAD file created at '{inputPath}'.");
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Inspect top‑level drawing entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    // Entities that support a Hyperlink property will have it as a string.
                    // Identify those where the Hyperlink is null or empty.
                    if (string.IsNullOrEmpty(entity.Hyperlink))
                    {
                        Console.WriteLine($"Entity Type: {entity.TypeName}, Layer: {entity.LayerName}, No Hyperlink");
                    }
                }

                // Inspect entities inside block definitions
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        if (string.IsNullOrEmpty(entity.Hyperlink))
                        {
                            Console.WriteLine($"Block: {block.Name}, Entity Type: {entity.TypeName}, Layer: {entity.LayerName}, No Hyperlink");
                        }
                    }
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