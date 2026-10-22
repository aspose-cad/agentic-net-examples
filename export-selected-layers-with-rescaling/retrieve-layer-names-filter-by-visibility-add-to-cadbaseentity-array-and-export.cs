using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadLayerExport
{
    class Program
    {
        static void Main(string[] args)
        {
            const string inputPath = "input.dwg";   // TODO: replace with actual input file path
            const string outputPath = "output.png"; // TODO: replace with desired output file path

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the CAD file
                using (Image image = Image.Load(inputPath))
                {
                    var cadImage = (CadImage)image;

                    // Retrieve visible layer names
                    var visibleLayerNames = new List<string>();
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        bool isOff = layer.ColorId < 0;
                        bool isFrozen = (layer.Flags & 1) != 0;
                        if (!isOff && !isFrozen)
                        {
                            visibleLayerNames.Add(layer.Name);
                        }
                    }

                    if (visibleLayerNames.Count == 0)
                    {
                        Console.WriteLine("No visible layers found. Export aborted.");
                        return;
                    }

                    // Export only the visible layers to PNG with rescaling
                    var rasterOptions = new CadRasterizationOptions
                    {
                        Layers = visibleLayerNames.ToArray(),
                        PageWidth = 1600,
                        PageHeight = 1200
                    };

                    var pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"Export completed: {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}