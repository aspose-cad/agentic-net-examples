using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.FileFormats.Cad;

namespace CadToPdfExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input CAD file path (DWG, DXF, etc.)
                string sourcePath = "input.dwg";
                // Output PDF file path
                string outputPath = "output.pdf";

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Load the CAD drawing
                using (CadImage cadImage = (CadImage)Image.Load(sourcePath))
                {
                    // Determine visible layers based on ColorId and Flags
                    List<string> visibleLayerNames = new List<string>();
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        bool isOff = layer.ColorId < 0;
                        bool isFrozen = (layer.Flags & 1) != 0; // Bit 0 indicates frozen
                        if (!isOff && !isFrozen)
                        {
                            visibleLayerNames.Add(layer.Name);
                        }
                    }

                    // If no layers are visible, skip export
                    if (visibleLayerNames.Count == 0)
                    {
                        Console.WriteLine("No visible layers found. Export aborted.");
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

                    // Save the drawing as PDF preserving layer visibility
                    cadImage.Save(outputPath, pdfOptions);
                    Console.WriteLine($"Export completed: {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}