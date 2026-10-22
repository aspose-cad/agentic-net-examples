using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadEntityFilterExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Input and output file paths
                string inputFilePath = "input.dwg";
                string outputFilePath = "output.dwg";

                // Guard against missing input file
                if (!File.Exists(inputFilePath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputFilePath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputFilePath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Layers to exclude from the drawing
                string[] excludedLayers = new[] { "Layer1", "Layer2" };

                // Load the CAD drawing
                using (Image image = Image.Load(inputFilePath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Create a new list to hold entities that are not on excluded layers
                    List<CadEntityBase> filteredEntities = new List<CadEntityBase>();

                    // Iterate over top‑level drawing entities
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Keep the entity if its layer is not in the excluded list
                        if (Array.IndexOf(excludedLayers, entity.LayerName) == -1)
                        {
                            filteredEntities.Add(entity);
                        }
                    }

                    // NOTE: Aspose.CAD exposes CadImage.Entities as a read‑only IEnumerable.
                    // Direct replacement of the entity collection is not supported by the SDK.
                    // Therefore the filtered list cannot be assigned back to cadImage.Entities.
                    // If modification of the drawing is required, entities must be manipulated
                    // within their containing blocks (CadBlockEntity) using the provided block APIs.

                    // Save the (unmodified) drawing to the output path
                    cadImage.Save(outputFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}