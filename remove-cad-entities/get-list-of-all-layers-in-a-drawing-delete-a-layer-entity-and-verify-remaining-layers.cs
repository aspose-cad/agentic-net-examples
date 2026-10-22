using System;
using System.Collections.Generic;
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
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // List all layers in the drawing
                Console.WriteLine("Layers in drawing:");
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    Console.WriteLine("- " + layer.Name);
                }

                // Specify the layer to delete
                string layerToDelete = "LayerToRemove";

                // Collect entities that belong to the specified layer
                var entitiesToRemove = new List<CadEntityBase>();
                foreach (var entity in cadImage.Entities)
                {
                    if (entity.LayerName == layerToDelete)
                    {
                        entitiesToRemove.Add(entity);
                    }
                }

                // Remove the collected entities by creating a new entity collection
                var remainingEntities = cadImage.Entities.Except(entitiesToRemove).ToArray();
                cadImage.Entities = remainingEntities;

                // Verify and display remaining layers (excluding the deleted one)
                Console.WriteLine("Remaining layers after deletion:");
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    if (!string.Equals(layer.Name, layerToDelete, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("- " + layer.Name);
                    }
                }

                // Save the modified drawing
                cadImage.Save(outputPath);
                Console.WriteLine($"Modified drawing saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}