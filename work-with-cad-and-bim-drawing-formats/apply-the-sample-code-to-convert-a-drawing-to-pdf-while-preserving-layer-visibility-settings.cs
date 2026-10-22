using System.Linq;
using System;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.FileFormats.Cad;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
                // Paths to the source DWG file and the destination PDF file
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

                    // Determine which layers are visible (off means ColorId < 0; frozen layers are still visible)
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
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        Layers = visibleLayerNames.ToArray()
                    };

                    // Set up PDF export options
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the drawing as PDF, preserving layer visibility
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