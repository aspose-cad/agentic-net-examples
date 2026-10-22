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
        const string inputPath = "input.dwg";
        const string outputPath = "output.png";

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

                Console.WriteLine("Available layouts:");
                foreach (var layoutEntry in cadImage.Layouts)
                {
                    CadLayout layout = layoutEntry.Value;
                    Console.WriteLine("- " + layout.LayoutName);
                }

                string selectedLayoutName = null;
                if (cadImage.Layouts.Count > 0)
                {
                    var firstLayout = cadImage.Layouts.First().Value;
                    selectedLayoutName = firstLayout.LayoutName;
                    Console.WriteLine("Selected layout: " + selectedLayoutName);
                }
                else
                {
                    Console.WriteLine("No layouts found in the drawing.");
                }

                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                if (selectedLayoutName != null)
                {
                    rasterOptions.Layouts = new[] { selectedLayoutName };
                }

                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                image.Save(outputPath, pngOptions);
                Console.WriteLine($"Layout exported to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}