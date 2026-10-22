using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg"; // TODO: replace with actual file path

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"File not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD‑specific members
                CadImage cadImage = (CadImage)image;

                // Retrieve the collection of layers
                var layersCollection = cadImage.Layers;

                // Iterate through all layers and output their names
                foreach (object layerObj in layersCollection)
                {
                    if (layerObj is CadLayerTable layer)
                    {
                        Console.WriteLine($"Layer Name: {layer.Name}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}