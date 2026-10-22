using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the source DWG file
            string sourcePath = "input.dwg";
            // Path for the exported PDF file
            string outputPath = "output.pdf";

            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            try
            {
                // Load the CAD drawing
                using (Image cadImageBase = Image.Load(sourcePath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)cadImageBase;

                    // Determine visible layers (exclude hidden/off, frozen, and non‑plottable layers)
                    var visibleLayerNames = new List<string>();
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        bool isOff = layer.ColorId < 0;
                        bool isFrozen = (layer.Flags & 1) != 0;
                        bool isPlottable = layer.PlotFlag ?? true;

                        if (!isOff && !isFrozen && isPlottable)
                        {
                            visibleLayerNames.Add(layer.Name);
                        }
                    }

                    // Configure rasterization options
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 595,   // A4 width in points
                        PageHeight = 842,  // A4 height in points
                        Layers = visibleLayerNames.ToArray()
                    };

                    // Set up PDF save options with the rasterization settings
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the drawing as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}