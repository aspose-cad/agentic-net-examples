using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputPath = "output.pdf";

            // Load CAD image as CadImage to access layers
            CadImage cadImage = (CadImage)Image.Load(inputPath);

            List<string> visibleLayerNames = new List<string>();
            foreach (CadLayerTable layer in cadImage.Layers)
            {
                bool isOff = layer.ColorId < 0;
                bool isFrozen = (layer.Flags & 1) != 0;
                if (!isOff && !isFrozen)
                {
                    visibleLayerNames.Add(layer.Name);
                }
            }

            if (visibleLayerNames.Count == 0)
            {
                Console.WriteLine("No visible layers to export.");
                return;
            }

            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                // Example page size (8.5 x 11 inches in points)
                PageWidth = 612,
                PageHeight = 792,
                // Render only the visible layers
                Layers = visibleLayerNames.ToArray()
            };

            PdfOptions pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            cadImage.Save(outputPath, pdfOptions);

            Console.WriteLine("Export completed.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}