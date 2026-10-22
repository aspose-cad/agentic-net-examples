using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                List<CadEntityBase> selectedEntities = new List<CadEntityBase>();

                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    if (block.Name.StartsWith("A", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            selectedEntities.Add(entity);
                        }

                        string outputPath = $"Block_{block.Name}.svg";
                        string outputDir = Path.GetDirectoryName(outputPath);
                        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                        {
                            Directory.CreateDirectory(outputDir);
                        }

                        SvgOptions svgOptions = new SvgOptions
                        {
                            VectorRasterizationOptions = new CadRasterizationOptions()
                        };

                        // Aspose.CAD does not provide direct entity‑level export, so we save the whole drawing.
                        cadImage.Save(outputPath, svgOptions);
                    }
                }

                Console.WriteLine($"Total selected entities: {selectedEntities.Count}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}