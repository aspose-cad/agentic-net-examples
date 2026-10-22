using System.Linq;
using System;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;
using System.IO;

namespace CadLayerVisibilityExport
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Input FBX file path
                    string inputPath = "input.fbx";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Output PDF file path
                    string outputPath = "output.pdf";

                    // Load the CAD drawing
                    using (Image image = Image.Load(inputPath))
                    {
                        // Cast to CadImage to access CAD-specific members
                        CadImage cadImage = (CadImage)image;

                        // Example: hide a specific layer by setting its ColorId to a negative value (off state)
                        foreach (CadLayerTable layer in cadImage.Layers)
                        {
                            if (layer.Name == "HiddenLayer")
                            {
                                // Off means ColorId < 0
                                layer.ColorId = -1;
                            }
                        }

                        // Build a list of layer names that are visible (ColorId >= 0)
                        List<string> visibleLayerNames = new List<string>();
                        foreach (CadLayerTable layer in cadImage.Layers)
                        {
                            bool isOff = layer.ColorId < 0;
                            if (!isOff)
                            {
                                visibleLayerNames.Add(layer.Name);
                            }
                        }

                        // If no layers are visible, skip exporting
                        if (visibleLayerNames.Count == 0)
                        {
                            Console.WriteLine("No visible layers to export.");
                            return;
                        }

                        // Configure rasterization options to include only visible layers
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                        rasterOptions.Layers = visibleLayerNames.ToArray();

                        // Set up PDF export options
                        PdfOptions pdfOptions = new PdfOptions();
                        pdfOptions.VectorRasterizationOptions = rasterOptions;

                        // Save the drawing as PDF
                        cadImage.Save(outputPath, pdfOptions);
                        Console.WriteLine($"Exported PDF saved to: {outputPath}");
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