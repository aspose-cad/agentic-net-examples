using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.ImageOptions;

class LayerExportExample
{
    static void Main()
    {
        // Input and output paths (replace with actual paths)
        string inputFile = @"C:\CAD\sample.dwg";
        string outputFile = @"C:\Export\sample.pdf";

        // Validate input file
        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"Input file not found: {inputFile}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputFile);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD drawing
            using (CadImage cadImage = (CadImage)Image.Load(inputFile))
            {
                // Identify layers that should be exported (visible and not frozen)
                List<string> exportLayers = new List<string>();
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    bool isOff = layer.ColorId < 0;                     // Off layers have negative color index
                    bool isFrozen = (layer.Flags & 1) != 0;            // Bit 0 of Flags indicates frozen state

                    if (!isOff && !isFrozen)
                    {
                        exportLayers.Add(layer.Name);
                    }
                }

                if (exportLayers.Count == 0)
                {
                    Console.WriteLine("No visible layers found to export.");
                    return;
                }

                // Configure rasterization options for PDF export
                CadRasterizationOptions rasterOpts = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,   // Correct enum spelling
                    LineScale = 0.5F,                  // Float literal
                    PageWidth = 2100F,                 // Float literal (e.g., 210 mm * 10)
                    PageHeight = 2970F,                // Float literal (e.g., 297 mm * 10)
                    Layers = exportLayers.ToArray()    // Export only the selected layers
                };

                // Set up PDF export options
                PdfOptions pdfOpts = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOpts
                };

                // Save the drawing to PDF using the configured options
                cadImage.Save(outputFile, pdfOpts);
                Console.WriteLine($"Export completed successfully: {outputFile}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during export: {ex.Message}");
        }
    }
}