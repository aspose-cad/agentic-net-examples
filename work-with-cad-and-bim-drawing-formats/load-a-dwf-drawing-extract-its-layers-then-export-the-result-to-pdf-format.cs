using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadLayerExtractionAndPdfExport
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the source DWF file
            string sourcePath = "input.dwf";

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
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                // Load the DWF drawing
                using (Image image = Image.Load(sourcePath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Extract and list layer names
                    foreach (CadLayerTable layerObj in cadImage.Layers)
                    {
                        if (layerObj is CadLayerTable layer)
                        {
                            Console.WriteLine($"Layer: {layer.Name}");
                        }
                    }

                    // Prepare PDF export options
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = new CadRasterizationOptions
                        {
                            PageWidth = 1600,
                            PageHeight = 1200
                        }
                    };

                    // Export the drawing to PDF
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