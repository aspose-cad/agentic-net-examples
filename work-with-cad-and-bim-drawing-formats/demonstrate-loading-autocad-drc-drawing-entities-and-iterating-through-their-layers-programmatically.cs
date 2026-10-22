using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadLayerIteration
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the AutoCAD drawing (DWG/DXF)
            string sourcePath = "sample.dwg";

            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            try
            {
                // Load the drawing using Aspose.CAD
                using (Image image = Image.Load(sourcePath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    Console.WriteLine("Layers in the drawing:");
                    foreach (CadLayerTable layerObj in cadImage.Layers)
                    {
                        // The enumeration yields objects; cast to CadLayerTable
                        if (layerObj is CadLayerTable layer)
                        {
                            Console.WriteLine($"- Layer: {layer.Name}");
                        }
                    }

                    Console.WriteLine();
                    Console.WriteLine("Top‑level entities and their layers:");
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Get the entity type name
                        string entityType = entity.TypeName.ToString();
                        // Get the layer name the entity belongs to
                        string layerName = entity.LayerName ?? "(No Layer)";

                        Console.WriteLine($"Entity: {entityType}, Layer: {layerName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}