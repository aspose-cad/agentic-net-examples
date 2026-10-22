using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Paths to the source and destination CAD files
            string inputPath = "input.dwg";
            string outputPath = "output.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("The file could not be loaded as a CAD image.");
                    return;
                }

                // -------------------------------------------------
                // Remove all line entities from the top‑level drawing
                // -------------------------------------------------
                var filteredEntities = cadImage.Entities.ToList();
                filteredEntities.RemoveAll(e => e is CadLine);

                // -------------------------------------------------
                // Remove entities that belong to hidden (off) layers
                // -------------------------------------------------
                var hiddenLayerNames = new List<string>();
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    // A layer is considered hidden when its ColorId is negative
                    if (layer.ColorId < 0)
                    {
                        hiddenLayerNames.Add(layer.Name);
                    }
                }

                filteredEntities.RemoveAll(e => e.LayerName != null && hiddenLayerNames.Contains(e.LayerName));

                // Assign the filtered collection back to the image (if possible)
                PropertyInfo entitiesProp = typeof(CadImage).GetProperty("Entities", BindingFlags.Public | BindingFlags.Instance);
                if (entitiesProp != null && entitiesProp.CanWrite)
                {
                    entitiesProp.SetValue(cadImage, filteredEntities);
                }
                else
                {
                    Console.WriteLine("Unable to set filtered entities back to CadImage. The changes may not be saved.");
                }

                // Save the modified drawing
                cadImage.Save(outputPath);
                Console.WriteLine($"Modified CAD file saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}