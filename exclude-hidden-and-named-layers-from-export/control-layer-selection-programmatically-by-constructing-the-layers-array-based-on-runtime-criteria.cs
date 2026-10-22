using System.Linq;
using System;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;
using System.IO;

namespace CadLayerSelectionExample
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Input CAD file path (replace with actual path)
                    string inputPath = "input.dwg";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Output raster image path
                    string outputPath = "output.png";

                    // Load the CAD file
                    using (Image image = Image.Load(inputPath))
                    {
                        // Cast to CadImage to access CAD-specific members
                        CadImage cadImage = (CadImage)image;

                        // Build a list of layer names that meet runtime criteria
                        // Example criteria: layer name starts with "A"
                        List<string> selectedLayerNames = new List<string>();
                        foreach (CadLayerTable layer in cadImage.Layers)
                        {
                            if (!string.IsNullOrEmpty(layer.Name) && layer.Name.StartsWith("A", StringComparison.OrdinalIgnoreCase))
                            {
                                selectedLayerNames.Add(layer.Name);
                            }
                        }

                        // If no layers match the criteria, skip exporting
                        if (selectedLayerNames.Count == 0)
                        {
                            Console.WriteLine("No layers matched the selection criteria. Export aborted.");
                            return;
                        }

                        // Configure rasterization options with the selected layers
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                        {
                            // Example page size; adjust as needed
                            PageWidth = 1600,
                            PageHeight = 1200,
                            // Assign the selected layers
                            Layers = selectedLayerNames.ToArray()
                        };

                        // Configure PNG export options
                        PngOptions pngOptions = new PngOptions
                        {
                            VectorRasterizationOptions = rasterOptions
                        };

                        // Save the rasterized image
                        cadImage.Save(outputPath, pngOptions);
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