using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source IGES file
                string sourcePath = "input.iges";

                // Path for the resulting PDF file
                string outputPath = "output.pdf";

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the IGES file
                using (Image cadImage = Image.Load(sourcePath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImg = (CadImage)cadImage;

                    // Example adjustment: ensure all layers are turned on (visible)
                    // In the current API, layer visibility is controlled via ColorId (negative = off).
                    // Setting a non‑negative ColorId makes the layer visible.
                    foreach (CadLayerTable layer in cadImg.Layers)
                    {
                        if (layer != null)
                        {
                            layer.ColorId = 7; // set to a default visible color index
                        }
                    }

                    // Example adjustment: modify line weight for all line entities
                    // NOTE: The current Aspose.CAD API does not expose a LineWeight property on CadLine.
                    // Therefore line weight cannot be modified programmatically.
                    foreach (var entity in cadImg.Entities)
                    {
                        if (entity is CadLine line)
                        {
                            // No LineWeight property available – operation not supported.
                            // Placeholder for future implementation if the SDK adds this feature.
                        }
                    }

                    // Prepare PDF export options
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = new CadRasterizationOptions
                        {
                            UnitType = UnitType.Centimenter,
                            LineScale = 1.0f
                        }
                    };

                    // Export the adjusted CAD drawing to PDF
                    cadImg.Save(outputPath, pdfOptions);
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