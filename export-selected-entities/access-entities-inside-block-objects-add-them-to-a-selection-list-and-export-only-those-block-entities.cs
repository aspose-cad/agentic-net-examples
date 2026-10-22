using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Paths
        string sourcePath = "input.dwg";
        string outputPath = "selected_entities.dxf";

        // Verify source file exists
        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        try
        {
            // Load the CAD image
            using (Image image = Image.Load(sourcePath))
            {
                // Cast to CadImage to access CAD-specific members
                if (image is not CadImage cadImage)
                {
                    Console.Error.WriteLine("Loaded file is not a CAD image.");
                    return;
                }

                // Prepare a list to hold selected entities from all blocks
                List<CadEntityBase> selectedEntities = new List<CadEntityBase>();

                // Iterate through each block in the drawing
                foreach (DictionaryEntry entry in cadImage.BlockEntities)
                {
                    if (entry.Value is CadBlockEntity block)
                    {
                        // Add all entities inside the block to the selection list
                        if (block.Entities != null)
                        {
                            selectedEntities.AddRange(block.Entities);
                        }
                    }
                }

                Console.WriteLine($"Total entities collected from blocks: {selectedEntities.Count}");

                // Limitation: Aspose.CAD does not provide a direct way to export only a subset of entities.
                // Selective export of the collected block entities cannot be performed with the current SDK.
                // Therefore, no output file will be generated for this operation.

                // If you still need to save the original drawing, uncomment the following lines:
                /*
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                var dxfOptions = new Aspose.CAD.ImageOptions.DxfOptions
                {
                    OutputMode = Aspose.CAD.ImageOptions.CadOutputMode.Convert,
                    DxfFileFormat = Aspose.CAD.FileFormats.Cad.CadConsts.CadFileFormat.Binary
                };

                cadImage.Save(outputPath, dxfOptions);
                Console.WriteLine($"Full drawing saved to {outputPath}");
                */
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}