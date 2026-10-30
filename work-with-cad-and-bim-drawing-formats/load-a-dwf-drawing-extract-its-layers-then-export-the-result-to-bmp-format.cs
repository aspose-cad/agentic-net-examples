using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Dwf;

namespace CadLayerExtraction
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source DWF file
                string sourcePath = "input.dwf";
                // Path for the exported BMP file
                string outputPath = "output.bmp";

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the DWF drawing
                using (Image image = Image.Load(sourcePath))
                {
                    DwfImage dwfImage = image as DwfImage;
                    if (dwfImage == null)
                    {
                        Console.Error.WriteLine("Failed to load DWF image.");
                        return;
                    }

                    // Extract and list layer information
                    var layerNames = dwfImage.Layers.GetLayersNames();
                    foreach (var name in layerNames)
                    {
                        Console.WriteLine($"Layer Name: {name}");
                    }

                    // Prepare BMP export options
                    BmpOptions bmpOptions = new BmpOptions
                    {
                        VectorRasterizationOptions = new CadRasterizationOptions
                        {
                            PageWidth = 1600,
                            PageHeight = 1200,
                            Layers = layerNames.ToArray()
                        }
                    };

                    // Save the drawing as BMP
                    image.Save(outputPath, bmpOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}