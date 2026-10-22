using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadLayerBatchExport
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Define input drawings and output folder
                    string[] inputFiles = new string[]
                    {
                        "drawing1.dwg",
                        "drawing2.dxf",
                        "drawing3.dwg"
                    };
                    string outputFolder = "output";

                    // Ensure output directory exists
                    if (!Directory.Exists(outputFolder))
                    {
                        Directory.CreateDirectory(outputFolder);
                    }

                    // Process each drawing
                    foreach (string inputPath in inputFiles)
                    {
                        // Load the CAD drawing
                        using (Image image = Image.Load(inputPath))
                        {
                            // Cast to CadImage to access CAD-specific members
                            CadImage cadImage = (CadImage)image;

                            // Collect names of layers that are neither off nor frozen
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

                            // If no layers are visible, skip exporting this drawing
                            if (visibleLayerNames.Count == 0)
                            {
                                Console.WriteLine($"No visible layers in '{inputPath}'. Skipping export.");
                                continue;
                            }

                            // Configure rasterization options to include only visible layers
                            CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                            rasterOptions.Layers = visibleLayerNames.ToArray();
                            rasterOptions.PageWidth = 800;   // Example width in pixels
                            rasterOptions.PageHeight = 600;  // Example height in pixels

                            // Choose PDF as the output format
                            PdfOptions pdfOptions = new PdfOptions();
                            pdfOptions.VectorRasterizationOptions = rasterOptions;

                            // Build output file path
                            string outputFileName = Path.GetFileNameWithoutExtension(inputPath) + ".pdf";
                            string outputPath = Path.Combine(outputFolder, outputFileName);

                            // Save the drawing with the specified options
                            cadImage.Save(outputPath, pdfOptions);
                            Console.WriteLine($"Exported '{inputPath}' to '{outputPath}'.");
                        }
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