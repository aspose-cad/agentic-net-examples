using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cadImage = (CadImage)image;

                int totalLayers = 0;

                // cadImage.Layers provides an enumerable of layer objects (CadLayerTable)
                foreach (CadLayerTable layerObj in cadImage.Layers)
                {
                    if (layerObj is CadLayerTable layer)
                    {
                        Console.WriteLine($"Layer: {layer.Name}");
                        totalLayers++;
                    }
                }

                Console.WriteLine($"Total number of layers: {totalLayers}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}