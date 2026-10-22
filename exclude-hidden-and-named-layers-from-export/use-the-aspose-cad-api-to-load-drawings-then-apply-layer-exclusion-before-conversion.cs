using System.Linq;
using System;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;
using System.IO;

namespace CadLayerExclusionExample
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Input and output file paths
                    string inputPath = "input.dwg";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        string outputPath = "output.pdf";

                    // Load the CAD drawing
                    using (Image image = Image.Load(inputPath))
                    {
                        // Cast to CadImage to access CAD-specific members
                        CadImage cadImage = (CadImage)image;

                        // Determine visible layers (exclude off or frozen layers)
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

                        // If no layers are visible, skip export
                        if (visibleLayerNames.Count == 0)
                        {
                            Console.WriteLine("No visible layers to export.");
                            return;
                        }

                        // Set up rasterization options with the selected layers
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                        rasterOptions.PageWidth = 1600;
                        rasterOptions.PageHeight = 1200;
                        rasterOptions.Layers = visibleLayerNames.ToArray();

                        // Choose PDF as the output format
                        PdfOptions pdfOptions = new PdfOptions();
                        pdfOptions.VectorRasterizationOptions = rasterOptions;

                        // Save the filtered drawing
                        cadImage.Save(outputPath, pdfOptions);
                        Console.WriteLine($"Drawing saved to {outputPath}");
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