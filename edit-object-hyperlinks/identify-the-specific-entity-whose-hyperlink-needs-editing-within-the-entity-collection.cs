using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

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

                // Iterate over top‑level drawing entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    // Check if the entity contains a hyperlink
                    if (!string.IsNullOrEmpty(entity.Hyperlink))
                    {
                        // Identify the specific entity by its current hyperlink value
                        if (entity.Hyperlink.Equals("http://old-link.com", StringComparison.OrdinalIgnoreCase))
                        {
                            // Edit the hyperlink
                            entity.Hyperlink = "https://new-link.com";
                            Console.WriteLine($"Hyperlink updated for entity type {entity.TypeName}");
                        }
                    }
                }

                // Save the modified drawing
                var saveOptions = new DxfOptions { OutputMode = CadOutputMode.Convert };
                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}