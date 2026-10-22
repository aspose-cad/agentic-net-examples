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
                    // Input CAD file path
                    string inputPath = "input.dwg";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Output PDF file path
                    string outputPath = "output.pdf";

                    // Load the CAD image
                    using (Image image = Image.Load(inputPath))
                    {
                        // Cast to CadImage to access CAD-specific members
                        CadImage cadImage = (CadImage)image;

                        // Collect names of layers that are not off and not frozen
                        List<string> visibleLayerNames = new List<string>();
                        foreach (CadLayerTable layer in cadImage.Layers)
                        {
                            bool isOff = layer.ColorId < 0;
                            bool isFrozen = (layer.Flags & 1) != 0; // Flag bit 0 indicates frozen
                            if (!isOff && !isFrozen)
                            {
                                visibleLayerNames.Add(layer.Name);
                            }
                        }

                        // If there are no visible layers, skip exporting
                        if (visibleLayerNames.Count == 0)
                        {
                            Console.WriteLine("No visible layers found. Export aborted.");
                            return;
                        }

                        // Configure rasterization options to include only visible layers
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                        rasterOptions.Layers = visibleLayerNames.ToArray();

                        // Set PDF save options and attach rasterization settings
                        PdfOptions pdfOptions = new PdfOptions();
                        pdfOptions.VectorRasterizationOptions = rasterOptions;

                        // Save the CAD drawing to PDF, exporting only the visible layers
                        cadImage.Save(outputPath, pdfOptions);
                        Console.WriteLine($"Export completed: {outputPath}");
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