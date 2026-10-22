using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main(string[] args)
    {
        try
        {
                string inputPath = "input.dwg";
                string outputPath = "output.png";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file '{inputPath}' does not exist.");
                    return;
                }

                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Gather names of visible layers (ColorId >= 0 and not off)
                    List<string> visibleLayerNames = new List<string>();
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        bool isOff = (layer.Flags & 1) != 0;
                        bool isVisible = layer.ColorId >= 0 && !isOff;
                        if (isVisible)
                        {
                            visibleLayerNames.Add(layer.Name);
                        }
                    }

                    // Configure rasterization options
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    rasterOptions.PageWidth = 1600;
                    rasterOptions.PageHeight = 1200;
                    rasterOptions.Layers = visibleLayerNames.ToArray(); // preserve layer inclusion list

                    // Note: Hatch rendering is enabled by default; no RenderHatches property exists.

                    // Configure PNG output options
                    PngOptions pngOptions = new PngOptions();
                    pngOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the rasterized image
                    cadImage.Save(outputPath, pngOptions);
                }

                Console.WriteLine("Export completed.");
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}