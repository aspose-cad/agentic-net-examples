using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Dwf;
using Aspose.CAD.FileFormats.Dwf.Whip.Objects;

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
                    // Cast to DwfImage to access DWF-specific members
                    DwfImage dwf = image as DwfImage;
                    if (dwf == null)
                    {
                        Console.Error.WriteLine("Failed to load DWF image.");
                        return;
                    }

                    // Extract and list layer names
                    List<string> layerNames = dwf.Layers.GetLayersNames();
                    Console.WriteLine("Layers found in the DWF file:");
                    foreach (string name in layerNames)
                    {
                        Console.WriteLine($"- {name}");
                    }

                    // Prepare PDF export options
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = new CadRasterizationOptions
                        {
                            PageWidth = 1600,
                            PageHeight = 1200,
                            // Export only the extracted layers
                            Layers = layerNames.ToArray()
                        }
                    };

                    // Export the drawing to PDF
                    dwf.Save(outputPath, pdfOptions);
                    Console.WriteLine($"PDF exported successfully to: {outputPath}");
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