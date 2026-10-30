using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main(string[] args)
    {
        // Input IFC file path (adjust as needed)
        string inputPath = "input/sample.ifc";

        // Verify source file exists
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Source IFC file not found: {inputPath}");
            return;
        }

        // Prepare output directory
        string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Output PNG file path
        string outputPath = Path.Combine(outputDir, "sample.png");

        try
        {
            // Load the file
            using (Image image = Image.Load(inputPath))
            {
                // Adjust CAD properties only if the image is a native CAD format
                if (image is CadImage cadImage)
                {
                    // Example property adjustment: set all entities to color ID 1 (red)
                    foreach (var entity in cadImage.Entities)
                    {
                        entity.ColorId = 1;
                    }

                    // Rename the first layer (if any)
                    int layerIndex = 0;
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        if (layerIndex == 0)
                        {
                            layer.Name = "ModifiedLayer";
                            break;
                        }
                        layerIndex++;
                    }

                    // Configure rasterization options for PNG output
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600f,
                        PageHeight = 1200f,
                        DrawType = CadDrawTypeMode.UseObjectColor,
                        BackgroundColor = Aspose.CAD.Color.White,
                        Layers = null // export all layers
                    };

                    // Set PNG save options with CAD rasterization
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the image as PNG
                    image.Save(outputPath, pngOptions);
                }
                else
                {
                    // For non‑CAD formats, save directly as PNG without CAD rasterization options
                    PngOptions pngOptions = new PngOptions();
                    image.Save(outputPath, pngOptions);
                }
            }

            Console.WriteLine($"PNG file saved successfully: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing file: {ex.Message}");
        }
    }
}