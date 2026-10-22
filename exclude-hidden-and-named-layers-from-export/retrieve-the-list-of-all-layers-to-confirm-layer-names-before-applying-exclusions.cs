using System.Linq;
using System;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;
using System.IO;

namespace CadLayerInspection
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Path to the source CAD file
                    string inputPath = "input.dwg";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Path to the output PDF file
                    string outputPath = "output.pdf";

                    // Load the CAD file
                    using (Image image = Image.Load(inputPath))
                    {
                        // Cast to CadImage to access CAD-specific members
                        CadImage cadImage = (CadImage)image;

                        // Retrieve and display all layer names
                        List<string> allLayerNames = new List<string>();
                        foreach (CadLayerTable layer in cadImage.Layers)
                        {
                            allLayerNames.Add(layer.Name);
                            Console.WriteLine($"Layer: {layer.Name}, ColorId: {layer.ColorId}, Flags: {layer.Flags}, PlotFlag: {layer.PlotFlag}");
                        }

                        // Define layers to exclude (example)
                        HashSet<string> excludedLayers = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                        {
                            "LayerToExclude1",
                            "LayerToExclude2"
                        };

                        // Build list of layers to include (all except excluded)
                        List<string> includeLayers = new List<string>();
                        foreach (string layerName in allLayerNames)
                        {
                            if (!excludedLayers.Contains(layerName))
                            {
                                includeLayers.Add(layerName);
                            }
                        }

                        // If no layers remain after exclusion, skip export
                        if (includeLayers.Count == 0)
                        {
                            Console.WriteLine("No layers left to export after applying exclusions.");
                            return;
                        }

                        // Set up rasterization options with the included layers
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                        {
                            // Example page size; adjust as needed
                            PageWidth = 1600,
                            PageHeight = 1200,
                            // Apply layer filtering
                            Layers = includeLayers.ToArray()
                        };

                        // Set up PDF save options
                        PdfOptions pdfOptions = new PdfOptions
                        {
                            VectorRasterizationOptions = rasterOptions
                        };

                        // Save the CAD drawing to PDF with the specified layer filter
                        cadImage.Save(outputPath, pdfOptions);
                        Console.WriteLine($"Export completed. Saved to {outputPath}");
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