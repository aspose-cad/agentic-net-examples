using System;
using System.IO;
using System.Linq;
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
            Console.WriteLine($"Input file '{inputPath}' does not exist.");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                var cadImage = (CadImage)image;

                // Retrieve the layer collection
                var layerList = cadImage.Layers.Cast<CadLayerTable>().ToList();

                Console.WriteLine("Layers in the drawing:");
                foreach (var layer in layerList)
                {
                    Console.WriteLine("- " + layer.Name);
                }

                // Remove the first entity that belongs to the first layer (if any)
                if (layerList.Count > 0)
                {
                    string targetLayerName = layerList[0].Name;

                    var entities = cadImage.Entities.Cast<CadEntityBase>().ToList();
                    int entityIndex = entities.FindIndex(e => e.LayerName == targetLayerName);

                    if (entityIndex >= 0)
                    {
                        entities.RemoveAt(entityIndex);
                        cadImage.Entities = entities.ToArray();
                        Console.WriteLine($"Removed entity at index {entityIndex} from layer '{targetLayerName}'.");
                    }
                    else
                    {
                        Console.WriteLine($"No entity found on layer '{targetLayerName}'.");
                    }
                }

                // Save the modified drawing
                cadImage.Save(outputPath);
                Console.WriteLine($"Modified drawing saved to '{outputPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}