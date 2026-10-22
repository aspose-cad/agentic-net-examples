using System.Linq;
using System;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;
using System.IO;

class Program
{
    static void Main()
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
        // Path for the exported PDF file
                string outputPath = "output.pdf";

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Collect names of layers that are intended to be exported
                    // Here we consider layers with a non‑negative ColorId (visible) and not frozen (Flags bit 0)
                    List<string> intendedLayers = new List<string>();
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        bool isVisible = layer.ColorId >= 0;
                        bool isFrozen = (layer.Flags & 1) != 0;
                        if (isVisible && !isFrozen)
                        {
                            intendedLayers.Add(layer.Name);
                        }
                    }

                    // If no intended layers are found, skip the export
                    if (intendedLayers.Count == 0)
                    {
                        Console.WriteLine("No intended layers found. Export aborted.");
                        return;
                    }

                    // Configure rasterization options to include only the intended layers
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    rasterOptions.Layers = intendedLayers.ToArray();

                    // Set up PDF export options
                    PdfOptions pdfOptions = new PdfOptions();
                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the drawing to PDF
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