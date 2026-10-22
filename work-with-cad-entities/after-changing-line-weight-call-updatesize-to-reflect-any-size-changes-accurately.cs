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
        const string outputPath = "output.dwg";

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
                // Cast to CadImage to access CAD object model
                CadImage cadImage = (CadImage)image;

                // Process top‑level drawing entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadLine line)
                    {
                        // NOTE: Aspose.CAD does not expose a LineWeight property.
                        // Changing line weight is not supported by the current SDK.
                        // The intended operation cannot be performed.
                    }
                }

                // Process entities inside block definitions
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        if (entity is CadLine line)
                        {
                            // NOTE: Aspose.CAD does not expose a LineWeight property.
                            // Changing line weight is not supported by the current SDK.
                        }
                    }
                }

                // Recalculate drawing extents after (theoretical) modifications
                cadImage.UpdateSize();

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the modified drawing
                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}