using System.Linq;
using System;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;
using System.IO;

namespace CadLayerFilteringExample
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

                        // Determine visible layers (ColorId >= 0 and not frozen)
                        List<string> visibleLayerNames = new List<string>();
                        foreach (CadLayerTable layer in cadImage.Layers)
                        {
                            bool isOff = layer.ColorId < 0;
                            bool isFrozen = (layer.Flags & 1) != 0; // Frozen flag is bit 0
                            if (!isOff && !isFrozen)
                            {
                                visibleLayerNames.Add(layer.Name);
                            }
                        }

                        // If no layers are visible, skip export
                        if (visibleLayerNames.Count == 0)
                        {
                            Console.WriteLine("No visible layers to export.");
                            return;
                        }

                        // Set up rasterization options with layer filtering
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                        {
                            // Export only the visible layers
                            Layers = visibleLayerNames.ToArray(),
                            // Optional: set page size or other options as needed
                            PageWidth = 1600,
                            PageHeight = 1200,
                            // Preserve original drawing without modifications
                            DrawType = CadDrawTypeMode.UseObjectColor
                        };

                        // Set up PDF save options
                        PdfOptions pdfOptions = new PdfOptions
                        {
                            VectorRasterizationOptions = rasterOptions
                        };

                        // Save the filtered drawing to PDF
                        cadImage.Save(outputPath, pdfOptions);
                        Console.WriteLine($"Exported visible layers to {outputPath}");
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