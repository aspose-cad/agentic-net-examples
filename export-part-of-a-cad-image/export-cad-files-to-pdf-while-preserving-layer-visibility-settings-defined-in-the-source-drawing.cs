using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;
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
            try
            {
                // Input CAD file path (replace with actual path)
                string inputPath = "input.dwg";

                // Verify input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Prepare output directory and file path
                string outputDirectory = "output";
                Directory.CreateDirectory(outputDirectory);
                string outputPath = Path.Combine(outputDirectory, "output.pdf");

                // Load CAD image
                using (Image img = Image.Load(inputPath))
                {
                    CadImage cadImage = img as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Loaded file is not a CAD image.");
                        return;
                    }

                    // Determine visible layers based on layer properties
                    List<string> visibleLayerNames = new List<string>();
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        bool isOff = layer.ColorId < 0;
                        bool isFrozen = (layer.Flags & 1) != 0;
                        bool plotFlag = layer.PlotFlag ?? true; // Treat null as true

                        if (!isOff && !isFrozen && plotFlag)
                        {
                            visibleLayerNames.Add(layer.Name);
                        }
                    }

                    // If no layers are visible, skip export
                    if (visibleLayerNames.Count == 0)
                    {
                        Console.Error.WriteLine("No visible layers found; export aborted.");
                        return;
                    }

                    // Set up PDF export options with layer filtering
                    PdfOptions pdfOptions = new PdfOptions();
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        Layers = visibleLayerNames.ToArray()
                    };
                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Save as PDF
                    cadImage.Save(outputPath, pdfOptions);
                    Console.WriteLine($"Export completed: {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}