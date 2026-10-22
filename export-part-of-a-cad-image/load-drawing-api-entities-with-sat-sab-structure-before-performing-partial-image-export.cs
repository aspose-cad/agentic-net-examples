using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace ExportCadPart
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // 1. Define the source CAD file and the folder where the result will be saved.
                string inputFilePath = "sample.dwg";
                string outputFolder = "Output";

                // 2. Ensure the output directory exists – create it if necessary.
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                // 3. Verify that the source file is present.
                if (!File.Exists(inputFilePath))
                {
                    Console.WriteLine($"Error: The file '{inputFilePath}' does not exist.");
                    return;
                }

                // 4. Load the CAD drawing.
                using (Image loadedImage = Image.Load(inputFilePath))
                {
                    CadImage cadImage = loadedImage as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("Error: Unable to cast loaded image to CadImage.");
                        return;
                    }

                    // 5. Determine visible layers.
                    List<string> visibleLayers = new List<string>();
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        bool isFrozen = (layer.Flags & 1) != 0;
                        bool isOff = layer.ColorId < 0;
                        if (!isFrozen && !isOff)
                        {
                            visibleLayers.Add(layer.Name);
                        }
                    }

                    // 6. If no visible layers were found we stop.
                    if (visibleLayers.Count == 0)
                    {
                        Console.WriteLine("No visible layers detected in the drawing. Export aborted.");
                        return;
                    }

                    // 7. Export only the first two visible layers (or fewer).
                    List<string> layersToExport = new List<string>();
                    for (int i = 0; i < visibleLayers.Count && i < 2; i++)
                    {
                        layersToExport.Add(visibleLayers[i]);
                    }

                    // 8. Configure rasterization options.
                    CadRasterizationOptions rasterOpts = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200,
                        Layers = layersToExport.ToArray()
                        // Layouts can be left as default (single page)
                    };

                    // 9. Create PNG export options.
                    PngOptions pngOpts = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOpts
                    };

                    // 10. Save the partial export.
                    string outputFilePath = Path.Combine(outputFolder, "PartialExport.png");
                    cadImage.Save(outputFilePath, pngOpts);

                    Console.WriteLine($"Export successful. File saved to: {outputFilePath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}