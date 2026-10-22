using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        string inputPath = "sample.dwg";
        string outputDir = "output";
        string outputPath = Path.Combine(outputDir, "exported.pdf");

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.WriteLine("Input file does not exist: " + inputPath);
                return;
            }

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                Console.WriteLine("Available layouts:");
                foreach (CadLayout layout in cadImage.Layouts.Values)
                {
                    Console.WriteLine("- " + layout.LayoutName);
                }

                CadLayout selectedLayout = null;
                foreach (CadLayout layout in cadImage.Layouts.Values)
                {
                    selectedLayout = layout;
                    break;
                }

                if (selectedLayout == null)
                {
                    Console.WriteLine("No layouts found in the CAD file.");
                    return;
                }

                List<string> layersToExport = new List<string>();
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    bool plot = true;
                    if (layer.PlotFlag.HasValue)
                    {
                        plot = layer.PlotFlag.Value;
                    }

                    if (plot)
                    {
                        layersToExport.Add(layer.Name);
                    }
                }

                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layouts = new[] { selectedLayout.LayoutName },
                    Layers = layersToExport.ToArray()
                };

                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine("Exported PDF saved to: " + outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}