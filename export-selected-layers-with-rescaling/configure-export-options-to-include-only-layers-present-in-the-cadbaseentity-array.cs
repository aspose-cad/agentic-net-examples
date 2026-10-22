using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            const string inputPath = "input.dwg";
            const string outputPath = "output.pdf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                using (Image cadImage = Image.Load(inputPath))
                {
                    CadImage cad = (CadImage)cadImage;

                    // Select a subset of entities (example: all entities)
                    CadEntityBase[] selectedEntities = cad.Entities.ToArray();

                    // Collect distinct layer names from the selected entities
                    HashSet<string> layerSet = new HashSet<string>();
                    foreach (CadEntityBase entity in selectedEntities)
                    {
                        if (!string.IsNullOrEmpty(entity.LayerName))
                        {
                            layerSet.Add(entity.LayerName);
                        }
                    }

                    string[] layersToExport = layerSet.ToArray();

                    // Configure rasterization options to include only the selected layers
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        Layers = layersToExport
                    };

                    // Use PDF as the output format
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the CAD drawing with only the specified layers
                    cadImage.Save(outputPath, pdfOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}