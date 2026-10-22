using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadLayoutLayerExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Expect three arguments: input CAD file path, output image file path, layout name to export
                if (args.Length < 3)
                {
                    Console.WriteLine("Usage: CadLayoutLayerExport <input.cad> <output.png> <layoutName>");
                    return;
                }

                string inputPath = args[0];
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                string outputPath = args[1];
                string layoutName = args[2];

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the CAD file
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Access layers to determine visibility (off if ColorId < 0, frozen if (Flags & 1) != 0)
                    List<string> visibleLayerNames = new List<string>();
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        bool isOff = layer.ColorId < 0;
                        bool isFrozen = (layer.Flags & 1) != 0;
                        if (!isOff && !isFrozen)
                        {
                            visibleLayerNames.Add(layer.Name);
                        }
                    }

                    // Note: CadRasterizationOptions does not provide a direct Layers filter.
                    // Layer visibility must be managed within the CAD file itself before rendering.
                    // The visibleLayerNames list can be used for custom processing if needed.

                    // Prepare rasterization options
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Set the layout to export
                        Layouts = new[] { layoutName },

                        // Optional: set image size (adjust as needed)
                        PageWidth = 1600,
                        PageHeight = 1200,
                        DrawType = CadDrawTypeMode.UseObjectColor
                    };

                    // Prepare PNG export options
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the rendered image
                    cadImage.Save(outputPath, pngOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}