using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main(string[] args)
    {
        try
        {
                // Validate arguments
                if (args.Length < 2)
                {
                    Console.WriteLine("Usage: <input-cad-file> <output-directory>");
                    return;
                }

                string inputPath = args[0];
                string outputDir = args[1];

                // Verify input file exists
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Ensure output directory exists
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the CAD image
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Iterate through each layout defined in the drawing
                    foreach (var layoutKvp in cadImage.Layouts)
                    {
                        string layoutName = layoutKvp.Key;
                        CadLayout layout = layoutKvp.Value;

                        // Retrieve entities that belong to the current layout.
                        // Entities expose LayoutTabName (or similar) to indicate their layout.
                        var layoutEntities = cadImage.Entities
                            .Where(e => string.Equals(e.LayoutTabName, layoutName, StringComparison.OrdinalIgnoreCase));

                        // Collect distinct layer names used by these entities.
                        var layerNames = layoutEntities
                            .Select(e => e.LayerName)          // LayerName is the property that holds the layer identifier.
                            .Where(name => !string.IsNullOrEmpty(name))
                            .Distinct()
                            .ToList();

                        // Example layer‑specific processing: output layer information.
                        Console.WriteLine($"Layout: {layoutName}");
                        foreach (string layer in layerNames)
                        {
                            // Placeholder for actual processing per layer.
                            Console.WriteLine($"  Layer: {layer}");
                        }

                        // Additional processing per layout can be inserted here.
                    }
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}