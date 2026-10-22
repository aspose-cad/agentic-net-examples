using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        // Input and output file paths
        string inputPath = "input.dxf";
        string outputPath = "output/modified.dxf";

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outDir) && !Directory.Exists(outDir))
            Directory.CreateDirectory(outDir);

        try
        {
            // Load CAD image
            using (var cadImage = (CadImage)Image.Load(inputPath))
            {
                // Get first layout (adjust selection as needed)
                CadLayout layout = cadImage.Layouts.Values.FirstOrDefault();
                if (layout == null)
                {
                    Console.WriteLine("No layout found in the CAD file.");
                    return;
                }

                // Index of the entity to remove (example: first entity)
                int targetIndex = 0;

                int entityCount = layout.Entities.Count();

                if (targetIndex < 0 || targetIndex >= entityCount)
                {
                    Console.WriteLine($"Index {targetIndex} is out of range. Layout contains {entityCount} entities.");
                }
                else
                {
                    // Create a new collection without the target entity
                    var newEntities = layout.Entities
                                            .Where((e, i) => i != targetIndex)
                                            .ToArray();

                    // Attempt to replace the Entities collection via reflection
                    var propInfo = typeof(CadLayout).GetProperty("Entities");
                    if (propInfo != null && propInfo.CanWrite)
                    {
                        propInfo.SetValue(layout, newEntities);
                        Console.WriteLine($"Entity at index {targetIndex} removed from layout \"{layout.LayoutName}\".");
                    }
                    else
                    {
                        Console.WriteLine("Removal of entities from a layout is not supported by the current Aspose.CAD API.");
                    }
                }

                // Save the modified CAD file
                cadImage.Save(outputPath, new DxfOptions());
                Console.WriteLine($"Modified CAD saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}