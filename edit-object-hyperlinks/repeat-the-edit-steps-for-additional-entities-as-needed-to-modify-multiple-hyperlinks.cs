using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
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

                // Iterate over all top‑level entities in the drawing
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    // Use reflection to determine whether the entity has a Hyperlink property
                    var hyperlinkProperty = entity.GetType().GetProperty("Hyperlink");
                    if (hyperlinkProperty != null && hyperlinkProperty.CanRead && hyperlinkProperty.CanWrite)
                    {
                        string currentHyperlink = hyperlinkProperty.GetValue(entity) as string;
                        if (!string.IsNullOrEmpty(currentHyperlink))
                        {
                            // Example modification: append a query string to the existing hyperlink
                            string updatedHyperlink = currentHyperlink + "?updated=true";
                            hyperlinkProperty.SetValue(entity, updatedHyperlink);
                        }
                    }
                }

                // Save the modified drawing back to DWG format
                var dwgOptions = new DwgOptions();
                cadImage.Save(outputPath, dwgOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}