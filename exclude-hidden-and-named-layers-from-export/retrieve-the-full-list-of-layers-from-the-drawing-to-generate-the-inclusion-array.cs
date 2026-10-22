using System.Linq;
using System;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;
using System.IO;

namespace CadLayerExtraction
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Path to the source CAD file (DWG, DXF, etc.)
                    string inputPath = "input.dwg";

            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Load the CAD drawing
                    using (Image cadImage = Image.Load(inputPath))
                    {
                        // Cast to CadImage to access CAD-specific members
                        CadImage cad = (CadImage)cadImage;

                        // Collect layer names into a list
                        List<string> layerNames = new List<string>();

                        // Enumerate all layers in the drawing
                        foreach (CadLayerTable layer in cad.Layers)
                        {
                            layerNames.Add(layer.Name);
                            Console.WriteLine($"Layer: {layer.Name}, ColorId: {layer.ColorId}, Flags: {layer.Flags}, PlotFlag: {layer.PlotFlag}");
                        }

                        // Convert the list to an array for inclusion in export options
                        string[] inclusionArray = layerNames.ToArray();

                        // Example: Export the drawing to PDF using only the retrieved layers
                        PdfOptions pdfOptions = new PdfOptions();
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                        {
                            // Specify which layers to include during rasterization
                            Layers = inclusionArray,
                            // Optional: set page size or other rasterization settings here
                        };
                        pdfOptions.VectorRasterizationOptions = rasterOptions;

                        string outputPath = "output.pdf";
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