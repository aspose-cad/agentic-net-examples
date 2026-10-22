using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main(string[] args)
    {
        // Input and output paths
        string inputPath = "input.dwg";
        string outputPath = "output.png";

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outDir) && !Directory.Exists(outDir))
        {
            Directory.CreateDirectory(outDir);
        }

        try
        {
            // Load CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Build list of layers that are not off (ColorId < 0) and not frozen (Flags bit 0)
                var visibleLayers = new List<string>();
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    bool isOff = layer.ColorId < 0;
                    bool isFrozen = (layer.Flags & 1) != 0;
                    if (!isOff && !isFrozen)
                    {
                        visibleLayers.Add(layer.Name);
                    }
                }

                // If no layers remain visible, skip export
                if (visibleLayers.Count == 0)
                {
                    Console.WriteLine("All layers are excluded; nothing to export.");
                    return;
                }

                // Configure rasterization options with high quality
                var rasterOptions = new CadRasterizationOptions
                {
                    // High quality for all rasterized elements
                    Quality = new RasterizationQuality
                    {
                        Text = RasterizationQualityValue.High,
                        Hatch = RasterizationQualityValue.High,
                        Arc = RasterizationQualityValue.High,
                        Ole = RasterizationQualityValue.High,
                        ObjectsPrecision = RasterizationQualityValue.High,
                        TextThicknessNormalization = true
                    },
                    // Unit type (Inch is valid)
                    UnitType = UnitType.Inch,
                    // Apply layer filter
                    Layers = visibleLayers.ToArray()
                };

                // Set PNG export options
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the rasterized image
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Export completed: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during processing: {ex.Message}");
        }
    }
}