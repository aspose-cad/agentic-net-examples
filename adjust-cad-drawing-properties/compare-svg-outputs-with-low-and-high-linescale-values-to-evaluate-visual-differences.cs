using System;
using System.IO;
using System.Collections;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputLow = "output_low.svg";
        string outputHigh = "output_high.svg";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Optional: inspect block entities
                foreach (DictionaryEntry blockEntry in cadImage.BlockEntities)
                {
                    if (blockEntry.Value is CadBlockEntity block)
                    {
                        Console.WriteLine($"Block name: {block.Name}");
                        foreach (var entity in block.Entities)
                        {
                            Console.WriteLine($"  Entity type: {entity.GetType().Name}");
                        }
                    }
                }

                // Save with low line scale
                var lowRasterOptions = new CadRasterizationOptions
                {
                    LineScale = 0.5f
                };
                var lowSvgOptions = new SvgOptions
                {
                    VectorRasterizationOptions = lowRasterOptions
                };
                cadImage.Save(outputLow, lowSvgOptions);

                // Save with high line scale
                var highRasterOptions = new CadRasterizationOptions
                {
                    LineScale = 2.0f
                };
                var highSvgOptions = new SvgOptions
                {
                    VectorRasterizationOptions = highRasterOptions
                };
                cadImage.Save(outputHigh, highSvgOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}
