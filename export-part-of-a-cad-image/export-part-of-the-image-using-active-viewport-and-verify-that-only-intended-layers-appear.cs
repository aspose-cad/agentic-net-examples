using System;
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
        const string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
            return;
        }

        try
        {
            using (Image cadImage = Image.Load(inputPath))
            {
                CadImage cad = (CadImage)cadImage;

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Active viewport is not exposed; default behavior will be used
                    Layers = new[] { "Layer1", "Layer2" },
                    PageWidth = 1600,
                    PageHeight = 1200
                    // Layouts can be set if a specific layout name is required
                };

                // Set PNG export options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Export the selected part of the drawing
                cadImage.Save(outputPath, pngOptions);

                // Verify that only the intended layers are present
                Console.WriteLine("Entities belonging to the exported layers:");
                foreach (CadEntityBase entity in cad.Entities)
                {
                    if (entity.LayerName == "Layer1" || entity.LayerName == "Layer2")
                    {
                        Console.WriteLine($"{entity.TypeName} on layer {entity.LayerName}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}