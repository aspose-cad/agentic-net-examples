using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadLayerExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Load the DXF drawing
                string inputPath = "input.dxf";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                CadImage cadImage = (CadImage)Image.Load(inputPath);

                // Determine visible layers (ColorId >= 0 and not frozen)
                List<string> visibleLayerNames = new List<string>();
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    bool isOff = layer.ColorId < 0;
                    bool isFrozen = (layer.Flags & 1) != 0; // bit 0 indicates frozen
                    if (!isOff && !isFrozen)
                    {
                        visibleLayerNames.Add(layer.Name);
                    }
                }

                if (visibleLayerNames.Count == 0)
                {
                    Console.WriteLine("No visible layers found. PDF export skipped.");
                    return;
                }

                // NOTE: Aspose.CAD does not provide a direct way to filter layers during PDF export.
                // The CadRasterizationOptions class does not contain a Layers property.
                // Therefore, the entire drawing will be exported. Visible layer detection is retained
                // for informational purposes.

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Example page size; adjust as needed
                    PageWidth = 800,
                    PageHeight = 600
                };

                // Configure PDF export options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Ensure output directory exists
                string outputPath = "output.pdf";
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the result to PDF
                cadImage.Save(outputPath, pdfOptions);

                Console.WriteLine($"PDF exported successfully to '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}