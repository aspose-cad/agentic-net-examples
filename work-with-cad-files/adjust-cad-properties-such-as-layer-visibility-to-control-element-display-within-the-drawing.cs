using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.ImageOptions;

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

        var outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Adjust layer visibility: hide layers named "HiddenLayer"
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    if (layer.Name.Equals("HiddenLayer", StringComparison.OrdinalIgnoreCase))
                    {
                        // Turn layer off by setting a negative ColorId
                        layer.ColorId = -1;
                        // Mark layer as frozen (Flags bit 0)
                        layer.Flags = (short)(layer.Flags | 1);
                    }
                    else
                    {
                        // Ensure layer is visible
                        if (layer.ColorId < 0)
                        {
                            layer.ColorId = 7; // default positive color index
                        }
                        // Clear frozen flag
                        layer.Flags = (short)(layer.Flags & ~1);
                    }
                }

                // Prepare rasterization options for export
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200
                };

                // Export the modified drawing to PNG
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
    }
}