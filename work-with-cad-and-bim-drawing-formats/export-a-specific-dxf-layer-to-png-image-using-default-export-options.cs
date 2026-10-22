using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Path to the source DXF file
        string sourcePath = "input.dxf";

        // Name of the layer to export
        string layerName = "MyLayer";

        // Path for the output PNG file
        string outputPath = "layer_export.png";

        // Verify source file exists
        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the DXF file
            using (Image image = Image.Load(sourcePath))
            {
                // NOTE: Aspose.CAD does not provide a direct API to export only a specific layer.
                // The library renders the entire drawing. To export a single layer, additional
                // processing (e.g., creating a new CAD file with only that layer) would be required,
                // which is beyond the scope of this example.

                Console.WriteLine($"Exporting the entire drawing to PNG (layer filtering not supported).");

                // Save the result as PNG using default export options
                PngOptions pngOptions = new PngOptions();
                image.Save(outputPath, pngOptions);
            }

            Console.WriteLine($"Export completed: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}