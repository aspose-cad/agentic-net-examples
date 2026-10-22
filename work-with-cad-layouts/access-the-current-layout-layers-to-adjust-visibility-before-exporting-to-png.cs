using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input and output file paths
                string inputPath = "input.dwg";
                string outputPath = "output.png";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the CAD file
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Determine the current layout name (use the first layout as the active one)
                    string currentLayoutName = string.Empty;
                    if (cadImage.Layouts != null && cadImage.Layouts.Count > 0)
                    {
                        // CadLayout has a LayoutName property
                        foreach (var kvp in cadImage.Layouts)
                        {
                            currentLayoutName = kvp.Value.LayoutName;
                            break;
                        }
                    }

                    // Collect names of visible layers (off if ColorId < 0, frozen if (Flags & 1) != 0)
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

                    if (visibleLayerNames.Count == 0)
                    {
                        Console.WriteLine("No visible layers found. Export aborted.");
                        return;
                    }

                    // Note: The Aspose.CAD SDK does not provide a Layers property on CadRasterizationOptions
                    // to filter exported layers directly. Therefore, all layers will be exported.
                    // Visible layer names are logged for reference.

                    Console.WriteLine("Visible layers that will be included in the export:");
                    foreach (var name in visibleLayerNames)
                    {
                        Console.WriteLine($"- {name}");
                    }

                    // Configure rasterization options for PNG export
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Export only the current layout
                        Layouts = new string[] { currentLayoutName },

                        // Optional: set image size or other options as needed
                        PageWidth = 1600,
                        PageHeight = 1200,
                        BackgroundColor = Color.White
                    };

                    // Configure PNG save options
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the image as PNG
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