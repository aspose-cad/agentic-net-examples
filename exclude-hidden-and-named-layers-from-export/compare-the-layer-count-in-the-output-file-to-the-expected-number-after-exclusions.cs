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
                    // Input CAD file path (DWG/DXF)
                    string inputPath = "input.dwg";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Output PDF file path
                    string outputPath = "output.pdf";

                    // Load the CAD file
                    using (Image image = Image.Load(inputPath))
                    {
                        // Cast to CadImage to access CAD-specific members
                        CadImage cadImage = (CadImage)image;

                        // Determine visible layers (not off and not frozen)
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

                        // Prepare rasterization options with the selected layers
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                        rasterOptions.Layers = visibleLayerNames.ToArray();

                        // Prepare PDF save options and assign rasterization options
                        PdfOptions pdfOptions = new PdfOptions();
                        pdfOptions.VectorRasterizationOptions = rasterOptions;

                        // Save the CAD drawing to PDF using only the visible layers
                        cadImage.Save(outputPath, pdfOptions);

                        // Compare layer count after exclusions
                        int expectedLayerCount = visibleLayerNames.Count;
                        Console.WriteLine($"Exported PDF contains {expectedLayerCount} visible layer(s).");
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