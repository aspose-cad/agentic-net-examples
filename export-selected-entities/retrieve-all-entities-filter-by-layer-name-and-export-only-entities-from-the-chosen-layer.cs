using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadLayerExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file (DWG/DXF)
                string inputFile = "input.dwg";

                // Desired output raster image file
                string outputFile = "layer_output.png";

                // Name of the layer to export
                string targetLayer = "MyLayer";

                // Verify input file exists
                if (!File.Exists(inputFile))
                {
                    Console.WriteLine($"Input file \"{inputFile}\" not found.");
                    return;
                }

                // Load the CAD drawing
                using (Image cadImage = Image.Load(inputFile))
                {
                    // Cast to CadImage to access CAD‑specific members
                    CadImage cad = (CadImage)cadImage;

                    // Verify that the target layer exists
                    bool layerExists = false;
                    foreach (CadLayerTable layer in cad.Layers)
                    {
                        if (layer.Name.Equals(targetLayer, StringComparison.OrdinalIgnoreCase))
                        {
                            layerExists = true;
                            break;
                        }
                    }

                    if (!layerExists)
                    {
                        Console.WriteLine($"Layer \"{targetLayer}\" not found in the drawing.");
                        return;
                    }

                    // Configure rasterization options to render only the chosen layer
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        Layers = new[] { targetLayer },
                        PageWidth = 1600,
                        PageHeight = 1200
                    };

                    // Set PNG save options and attach the rasterization settings
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the filtered drawing to PNG
                    cadImage.Save(outputFile, pngOptions);
                    Console.WriteLine($"Layer \"{targetLayer}\" exported to \"{outputFile}\".");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}