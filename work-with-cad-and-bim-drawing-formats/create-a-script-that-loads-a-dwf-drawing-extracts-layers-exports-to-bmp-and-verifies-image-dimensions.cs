using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
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
            using (Image cadImage = Image.Load(sourcePath))
            {
                CadImage cad = (CadImage)cadImage;

                // Extract distinct layer names from the drawing
                HashSet<string> layerNames = new HashSet<string>();
                foreach (CadEntityBase entity in cad.Entities)
                {
                    if (!string.IsNullOrEmpty(entity.LayerName))
                    {
                        layerNames.Add(entity.LayerName);
                    }
                }

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
                    }
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
                cad.Save(bmpPath, bmpOptions);
            }

            // Verify the dimensions of the exported BMP image
            if (!File.Exists("output/output.bmp"))
            {
                Console.Error.WriteLine("Exported BMP file not found.");
                return;
            }

            using (Image bmpImage = Image.Load("output/output.bmp"))
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