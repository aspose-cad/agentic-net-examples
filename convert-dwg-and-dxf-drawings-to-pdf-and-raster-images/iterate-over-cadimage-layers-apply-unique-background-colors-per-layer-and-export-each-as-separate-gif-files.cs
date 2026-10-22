using Aspose.CAD;
using System;
using System.IO;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.FileFormats.Cad;

class LayerExportSample
{
    static void Main(string[] args)
    {
        // Input CAD file path
        string inputPath = "input.dwg";

        // Output directory for GIF files
        string outputDir = "ExportedLayers";

        // Validate input file
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

        try
        {
            // Load CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Counter for generating unique background colors (as layer color IDs)
                short colorIdCounter = 1;

                // Iterate over each layer
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    // Assign a unique color ID to the layer (background effect for this sample)
                    layer.ColorId = colorIdCounter;
                    colorIdCounter++;

                    // Prepare rasterization options to render only the current layer
                    var rasterOptions = new CadRasterizationOptions
                    {
                        // Set the layer to be rendered
                        Layers = new string[] { layer.Name },

                        // Optional: set other rendering options as needed
                        // For example, you can control line weight, scaling, etc.
                    };

                    // Create GIF export options
                    var gifOptions = new GifOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Build output file name
                    string outputPath = Path.Combine(outputDir, $"{layer.Name}.gif");

                    // Export the current layer to GIF
                    cadImage.Save(outputPath, gifOptions);

                    Console.WriteLine($"Exported layer '{layer.Name}' to {outputPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}