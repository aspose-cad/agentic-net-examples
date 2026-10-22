using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dxf";
        const string outputPath = "Layer1.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
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
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cadImage = (CadImage)image;

                // Name of the layer to export
                string targetLayer = "Layer1";

                // Keep only entities that belong to the specified layer
                cadImage.Entities = cadImage.Entities
                    .Where(entity => string.Equals(entity.LayerName, targetLayer, StringComparison.OrdinalIgnoreCase))
                    .ToArray();

                // Save the filtered drawing to an image (PNG) using default settings
                PngOptions pngOptions = new PngOptions();
                cadImage.Save(outputPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}