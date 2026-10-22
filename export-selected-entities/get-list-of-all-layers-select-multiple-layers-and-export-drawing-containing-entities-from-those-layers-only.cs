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
                // Path to the source CAD file
                string inputPath = "input.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Retrieve all layer names in the drawing
                    List<string> allLayerNames = new List<string>();
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        // Assuming each item has a Name property
                        allLayerNames.Add(layer.Name);
                    }

                    // Example: select specific layers by name (modify as needed)
                    List<string> selectedLayers = new List<string>();
                    foreach (string layerName in allLayerNames)
                    {
                        // Add layers that start with "A" or "B" as an example filter
                        if (layerName.StartsWith("A") || layerName.StartsWith("B"))
                        {
                            selectedLayers.Add(layerName);
                        }
                    }

                    // Configure rasterization options to render only the selected layers
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        Layers = selectedLayers.ToArray(), // layers to include
                        PageWidth = 1600,   // optional: output width
                        PageHeight = 1200,  // optional: output height
                        Layouts = new string[] { "Model" } // optional: layout name
                    };

                    // Choose an output format (PNG in this example)
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the filtered drawing
                    string outputPath = "output.png";
                    cadImage.Save(outputPath, pngOptions);
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