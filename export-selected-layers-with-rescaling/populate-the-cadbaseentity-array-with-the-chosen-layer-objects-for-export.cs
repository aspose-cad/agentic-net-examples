using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        // Paths and target layer name
        string inputPath = @"C:\CAD\input.dwg";
        string outputPath = @"C:\CAD\output.pdf";
        string targetLayer = "Layer1";

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input CAD file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string? outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load CAD image
            CadImage cadImage = (CadImage)Image.Load(inputPath);

            // Select entities that belong to the specified layer
            CadEntityBase[] layerEntities = cadImage.Entities
                .Where(e => string.Equals(e.LayerName, targetLayer, StringComparison.OrdinalIgnoreCase))
                .ToArray();

            // At this point 'layerEntities' holds all objects from the chosen layer
            // (Further processing or export can be performed using this array.)

            // Example: Export the whole drawing to PDF (optional)
            var pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new[] { "Model" }
                }
            };
            cadImage.Save(outputPath, pdfOptions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}