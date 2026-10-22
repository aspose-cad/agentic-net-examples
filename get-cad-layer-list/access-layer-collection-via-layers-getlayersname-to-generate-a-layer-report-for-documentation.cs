using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main(string[] args)
    {
        string inputPath = "sample.dwg";
        string outputDir = "output";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to load CAD image.");
                    return;
                }

                // Collect distinct layer names from entities
                HashSet<string> layerNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (!string.IsNullOrEmpty(entity.LayerName))
                    {
                        layerNames.Add(entity.LayerName);
                    }
                }

                // Generate layer report
                string reportPath = Path.Combine(outputDir, "LayerReport.txt");
                using (StreamWriter writer = new StreamWriter(reportPath))
                {
                    writer.WriteLine("Layer Report");
                    writer.WriteLine("============");
                    foreach (string layer in layerNames)
                    {
                        writer.WriteLine(layer);
                    }
                }

                Console.WriteLine($"Layer report generated at: {reportPath}");

                // Save a copy of the CAD file in DXF format
                string dxfPath = Path.Combine(outputDir, "Copy.dxf");
                DxfOptions dxfOptions = new DxfOptions();
                cadImage.Save(dxfPath, dxfOptions);
                Console.WriteLine($"CAD file saved as DXF at: {dxfPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}