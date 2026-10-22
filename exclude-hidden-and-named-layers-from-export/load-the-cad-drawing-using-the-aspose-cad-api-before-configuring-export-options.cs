using System.Linq;
using System;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;
using System.IO;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Input CAD file path (DWG/DXF)
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

                        // Determine visible layers (not off and not frozen)
                        List<string> visibleLayers = new List<string>();
                        foreach (CadLayerTable layer in cadImage.Layers)
                        {
                            bool isOff = layer.ColorId < 0;
                            bool isFrozen = (layer.Flags & 1) != 0; // Flag bit 0 indicates frozen

                            if (!isOff && !isFrozen)
                            {
                                visibleLayers.Add(layer.Name);
                            }
                        }

                        // If no layers are visible, skip exporting
                        if (visibleLayers.Count == 0)
                        {
                            Console.WriteLine("No visible layers found. Export aborted.");
                            return;
                        }

                        // Configure rasterization options for vector export
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                        rasterOptions.Layers = visibleLayers.ToArray();

                        // Configure PDF export options
                        PdfOptions pdfOptions = new PdfOptions();
                        pdfOptions.VectorRasterizationOptions = rasterOptions;

                        // Save the drawing as PDF using the configured options
                        cadImage.Save(outputPath, pdfOptions);
                        Console.WriteLine($"CAD drawing exported to PDF: {outputPath}");
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