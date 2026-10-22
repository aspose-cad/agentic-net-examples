using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadLayerVisibilityExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file
                string sourcePath = "input.dwg";
                // Path for the exported PDF
                string outputPath = "output.pdf";

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Load the CAD file
                using (Image image = Image.Load(sourcePath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Collect names of visible layers (not off and not frozen)
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
                        PageWidth = 1600,
                        PageHeight = 1200,
                        Layers = visibleLayerNames.ToArray()
                    };

                    // Set up PDF export options
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the filtered drawing to PDF
                    cadImage.Save(outputPath, pdfOptions);
                    Console.WriteLine($"Export completed. Visible layers exported to '{outputPath}'.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}