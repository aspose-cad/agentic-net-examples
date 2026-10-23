using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Dwf;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source DWF file
            string sourcePath = "sample.dwf";

            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            // Load the DWF drawing
            using (Image image = Image.Load(sourcePath))
            {
                DwfImage dwf = (DwfImage)image;

                // Extract layer names from the drawing
                // GetLayersNames may return List<string>; convert to array for rasterization options
                List<string> layerNamesList = dwf.Layers.GetLayersNames();
                string[] layerNames = layerNamesList?.ToArray() ?? Array.Empty<string>();

                Console.WriteLine("Layers found in the drawing:");
                foreach (string name in layerNames)
                {
                    Console.WriteLine(name);
                }

                // Configure rasterization options for BMP export
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,
                    LineScale = 1.0f,
                    PageWidth = 800,
                    PageHeight = 600,
                    Margins = new Margins
                    {
                        Left = 10,
                        Right = 10,
                        Top = 10,
                        Bottom = 10
                    },
                    Layers = layerNames // Export only the extracted layers
                };

                // Set up BMP export options
                BmpOptions bmpOptions = new BmpOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Path for the exported BMP file
                string outputDir = "output";
                Directory.CreateDirectory(outputDir);
                string bmpPath = Path.Combine(outputDir, "output.bmp");

                // Save the drawing as BMP
                dwf.Save(bmpPath, bmpOptions);
            }

            // Verify the dimensions of the exported BMP image
            string exportedBmpPath = Path.Combine("output", "output.bmp");
            if (!File.Exists(exportedBmpPath))
            {
                Console.Error.WriteLine("Exported BMP file not found.");
                return;
            }

            using (Image bmpImage = Image.Load(exportedBmpPath))
            {
                Console.WriteLine($"Exported BMP dimensions: {bmpImage.Width} x {bmpImage.Height}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}