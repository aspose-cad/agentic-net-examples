using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dxf";
        const string outputPath = "output.pdf";
        const string layoutName = "Layout1";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        var outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the DXF file
            CadImage cadImage = (CadImage)Image.Load(inputPath);

            // Collect names of layers that are not off (ColorId < 0) and not frozen ((Flags & 1) != 0)
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

            // Set up rasterization options for PDF export, limiting to the chosen layout and visible layers
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                Layouts = new[] { layoutName },
                Layers = visibleLayerNames.ToArray()
            };

            // Set up PDF options
            PdfOptions pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Export to PDF
            cadImage.Save(outputPath, pdfOptions);

            // Clean up
            cadImage.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}