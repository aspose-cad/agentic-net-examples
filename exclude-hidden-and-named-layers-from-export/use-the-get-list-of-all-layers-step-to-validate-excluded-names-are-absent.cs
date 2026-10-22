using System.Linq;
using System;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;
using System.IO;

namespace AsposeCadExample
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

                        // Enumerate and display layer information
                        foreach (CadLayerTable layer in cadImage.Layers)
                        {
                            Console.WriteLine($"Layer: {layer.Name}, ColorId: {layer.ColorId}, Flags: {layer.Flags}, PlotFlag: {layer.PlotFlag}");
                        }

                        // Determine visible layers (not off and not frozen)
                        List<string> visibleLayerNames = new List<string>();
                        foreach (CadLayerTable layer in cadImage.Layers)
                        {
                            bool isOff = layer.ColorId < 0;
                            bool isFrozen = (layer.Flags & 1) != 0;
                            if (!isOff && !isFrozen)
                            {
                                visibleLayerNames.Add(layer.Name);
                            }
                        }

                        // Configure rasterization options for PDF export
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                        rasterOptions.PageWidth = 1600;
                        rasterOptions.PageHeight = 1200;
                        rasterOptions.Layers = visibleLayerNames.ToArray();

                        // Set PDF save options
                        PdfOptions pdfOptions = new PdfOptions();
                        pdfOptions.VectorRasterizationOptions = rasterOptions;

                        // Save the drawing as PDF, exporting only visible layers
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