using System.Linq;
using System;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;
using System.IO;

namespace CadLayerExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Input CAD file path
                    string inputPath = "input.dwg";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Output PDF file path
                    string outputPath = "output.pdf";

                    // Layers to exclude from export
                    string[] excludedLayers = new string[] { "LayerToExclude1", "LayerToExclude2" };

                    // Load the CAD image
                    using (Image cadImage = Image.Load(inputPath))
                    {
                        // Cast to CadImage to access CAD-specific members
                        CadImage cad = (CadImage)cadImage;

                        // Collect names of layers that are not excluded
                        List<string> includedLayerNames = new List<string>();
                        foreach (CadLayerTable layer in cad.Layers)
                        {
                            bool isExcluded = false;
                            foreach (string excl in excludedLayers)
                            {
                                if (string.Equals(layer.Name, excl, StringComparison.OrdinalIgnoreCase))
                                {
                                    isExcluded = true;
                                    break;
                                }
                            }

                            if (!isExcluded)
                            {
                                includedLayerNames.Add(layer.Name);
                            }
                        }

                        // Configure rasterization options with the filtered layer list
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                        {
                            // Set desired page size or other options as needed
                            PageWidth = 1600,
                            PageHeight = 1200,
                            // Include only the selected layers
                            Layers = includedLayerNames.ToArray()
                        };

                        // Set PDF save options using the rasterization settings
                        PdfOptions pdfOptions = new PdfOptions
                        {
                            VectorRasterizationOptions = rasterOptions
                        };

                        // Save the CAD drawing to PDF, rendering only the included layers
                        cadImage.Save(outputPath, pdfOptions);
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