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
        const string outputPath = "output.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Set a global default font
                const string globalDefaultFont = "Arial";
                cadImage.DefaultFont = globalDefaultFont;

                // Iterate through layers and apply layer‑specific fonts
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    // Choose font for the current layer
                    string layerFont = layer.Name.Equals("Title", StringComparison.OrdinalIgnoreCase)
                        ? "Times New Roman"
                        : globalDefaultFont;

                    // Apply the font for processing this layer
                    string previousFont = cadImage.DefaultFont;
                    cadImage.DefaultFont = layerFont;

                    // Process entities belonging to the current layer
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        if (entity.LayerName == layer.Name)
                        {
                            Console.WriteLine($"Layer: {layer.Name}, Entity Type: {entity.TypeName}");
                        }
                    }

                    // Restore the previous font before moving to the next layer
                    cadImage.DefaultFont = previousFont;
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