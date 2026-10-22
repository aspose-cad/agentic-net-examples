using System.Linq;
using System;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;
using System.IO;

namespace CadLayerFilterExample
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

                    // Load the CAD drawing
                    using (Image image = Image.Load(inputPath))
                    {
                        // Cast to CadImage to access CAD-specific members
                        CadImage cadImage = (CadImage)image;

                        // Prepare a list for names of layers that should remain visible
                        List<string> visibleLayerNames = new List<string>();

                        // Iterate through all layers in the drawing
                        foreach (CadLayerTable layer in cadImage.Layers)
                        {
                            // Hide the layer named "LayerToHide" by setting its ColorId to a negative value (off)
                            if (layer.Name == "LayerToHide")
                            {
                                layer.ColorId = -1; // Off state
                                continue;
                            }

                            // Add other layer names to the visible list
                            visibleLayerNames.Add(layer.Name);
                        }

                        // Configure rasterization options for vector export (PDF)
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                        {
                            // Export only the layers collected above
                            Layers = visibleLayerNames.ToArray(),
                            // Optional page size settings
                            PageWidth = 1600,
                            PageHeight = 1200
                        };

                        // Set PDF save options and attach the rasterization options
                        PdfOptions pdfOptions = new PdfOptions
                        {
                            VectorRasterizationOptions = rasterOptions
                        };

                        // Save the filtered drawing to PDF
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