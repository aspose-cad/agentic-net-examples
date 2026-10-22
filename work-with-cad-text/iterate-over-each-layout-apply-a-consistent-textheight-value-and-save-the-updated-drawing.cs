using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDir = "output";
            Directory.CreateDirectory(outputDir);

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Loaded file is not a CAD image.");
                    return;
                }

                // Desired uniform text height
                const double uniformTextHeight = 2.5; // adjust as needed

                // Apply the text height to all text entities in the drawing
                foreach (var entity in cadImage.Entities)
                {
                    if (entity is CadText textEntity)
                    {
                        // Set the text height; property exists on CadTextEntity
                        textEntity.TextHeight = uniformTextHeight;
                    }
                }

                // Iterate over each layout and save as PNG
                foreach (KeyValuePair<string, CadLayout> kvp in cadImage.Layouts)
                {
                    string layoutName = kvp.Key;

                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        Layouts = new[] { layoutName }
                    };

                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    string outputPath = Path.Combine(outputDir, $"{layoutName}.png");
                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"Saved layout '{layoutName}' to {outputPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}