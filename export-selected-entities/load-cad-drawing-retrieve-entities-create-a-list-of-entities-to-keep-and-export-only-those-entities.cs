using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

namespace ExportSelectedEntities
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the source CAD file (DWG or DXF)
            string sourcePath = "input.dwg";
            // Path for the exported CAD file containing only selected entities
            string outputPath = "selected_entities.dxf";

            if (!File.Exists(sourcePath))
            {
                Console.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            try
            {
                // Load the CAD drawing
                using (CadImage cadImage = (CadImage)Image.Load(sourcePath))
                {
                    // Example criterion: keep only line entities
                    IEnumerable<CadEntityBase> selectedEntities = cadImage.Entities
                        .Where(e => e is CadLine);

                    // Access the Model Space block where most drawing entities reside
                    const string modelSpaceName = "*Model_Space";
                    if (!cadImage.BlockEntities.ContainsKey(modelSpaceName))
                    {
                        Console.WriteLine("Model space block not found.");
                        return;
                    }

                    // Retrieve the model space block entity
                    if (cadImage.BlockEntities[modelSpaceName] is not CadBlockEntity modelSpaceBlock)
                    {
                        Console.WriteLine("Failed to cast model space block.");
                        return;
                    }

                    // Clear existing entities from the model space block
                    modelSpaceBlock.Entities.Clear();

                    // Add the selected entities back into the model space block
                    foreach (CadEntityBase entity in selectedEntities)
                    {
                        modelSpaceBlock.AddEntity(entity);
                    }

                    // Save the modified drawing to a new file (DXF format)
                    var dxfOptions = new DxfOptions
                    {
                        // Convert mode renders geometry as lines
                        OutputMode = CadOutputMode.Convert
                    };

                    cadImage.Save(outputPath, dxfOptions);
                }

                Console.WriteLine("Export of selected entities completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}