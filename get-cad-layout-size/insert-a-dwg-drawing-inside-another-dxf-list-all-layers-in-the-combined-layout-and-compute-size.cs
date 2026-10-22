using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output paths (replace with actual paths)
            string basePath = @"C:\CAD";
            string targetPath = Path.Combine(basePath, "target.dxf");   // DXF that will receive the DWG
            string sourcePath = Path.Combine(basePath, "source.dwg");   // DWG to be inserted
            string outputPath = Path.Combine(basePath, "combined.dxf"); // Resulting combined file

            // Verify that both source files exist
            if (!File.Exists(targetPath) || !File.Exists(sourcePath))
            {
                Console.WriteLine("One or both input files were not found.");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Load the target DXF
            using (CadImage targetImage = (CadImage)Image.Load(targetPath))
            {
                // Load the source DWG
                using (CadImage sourceImage = (CadImage)Image.Load(sourcePath))
                {
                    // Get the Model Space block for both drawings
                    var targetModel = targetImage.BlockEntities["*Model_Space"] as CadBlockEntity;
                    var sourceModel = sourceImage.BlockEntities["*Model_Space"] as CadBlockEntity;

                    if (targetModel == null || sourceModel == null)
                    {
                        Console.WriteLine("Unable to locate Model Space block in one of the drawings.");
                        return;
                    }

                    // Copy every entity from the source drawing into the target drawing
                    foreach (var entity in sourceModel.Entities)
                    {
                        targetModel.AddEntity(entity);
                    }

                    // List all distinct layer names present in the combined drawing
                    var layerNames = new HashSet<string>();
                    foreach (var entity in targetModel.Entities)
                    {
                        if (!string.IsNullOrEmpty(entity.LayerName))
                            layerNames.Add(entity.LayerName);
                    }

                    Console.WriteLine("Layers in the combined drawing:");
                    foreach (var name in layerNames)
                    {
                        Console.WriteLine($"- {name}");
                    }

                    // Compute the overall size of the combined drawing using its bounds
                    var bounds = targetImage.Bounds; // Rectangle
                    double width = bounds.Width;
                    double height = bounds.Height;
                    double depth = 0; // Depth not applicable for 2D drawings

                    Console.WriteLine($"Drawing size: Width = {width}, Height = {height}, Depth = {depth}");

                    // Save the combined drawing as DXF
                    var saveOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };
                    targetImage.Save(outputPath);
                    Console.WriteLine($"Combined drawing saved to: {outputPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}