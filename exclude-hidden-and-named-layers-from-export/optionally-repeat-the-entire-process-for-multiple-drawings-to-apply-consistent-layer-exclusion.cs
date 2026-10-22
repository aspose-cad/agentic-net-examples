using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadLayerExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input CAD files
                string[] inputFiles = new string[]
                {
                    "drawing1.dwg",
                    "drawing2.dwg"
                };

                // Layers that should be excluded from the export
                string[] layersToExclude = new string[]
                {
                    "Layer1",
                    "Layer2"
                };

                foreach (string inputPath in inputFiles)
                {
                    if (!File.Exists(inputPath))
                    {
                        Console.WriteLine($"Input file not found: {inputPath}");
                        continue;
                    }

                    // Load the CAD drawing
                    using (Image image = Image.Load(inputPath))
                    {
                        CadImage cadImage = image as CadImage;
                        if (cadImage == null)
                        {
                            Console.WriteLine($"Failed to cast loaded image to CadImage for file: {inputPath}");
                            continue;
                        }

                        // Determine visible layers (exclude specified layers and off/frozen layers)
                        List<string> visibleLayers = new List<string>();
                        foreach (CadLayerTable layer in cadImage.Layers)
                        {
                            bool isExcluded = Array.IndexOf(layersToExclude, layer.Name) >= 0;
                            bool isOff = layer.ColorId < 0;
                            bool isFrozen = (layer.Flags & 1) != 0; // Bit 0 indicates frozen

                            if (!isExcluded && !isOff && !isFrozen)
                            {
                                visibleLayers.Add(layer.Name);
                            }
                        }

                        // If no layers remain, skip exporting this drawing
                        if (visibleLayers.Count == 0)
                        {
                            Console.WriteLine($"No visible layers to export for file: {inputPath}");
                            continue;
                        }

                        // Set up rasterization options with the selected layers
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                        {
                            Layers = visibleLayers.ToArray()
                        };

                        // Configure PDF export options
                        PdfOptions pdfOptions = new PdfOptions
                        {
                            VectorRasterizationOptions = rasterOptions
                        };

                        // Determine output file path
                        string outputPath = Path.ChangeExtension(inputPath, ".pdf");
                        string outputDir = Path.GetDirectoryName(outputPath);
                        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                        {
                            Directory.CreateDirectory(outputDir);
                        }

                        // Save the drawing as PDF
                        cadImage.Save(outputPath, pdfOptions);
                        Console.WriteLine($"Exported '{inputPath}' to '{outputPath}' with {visibleLayers.Count} layer(s).");
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
}