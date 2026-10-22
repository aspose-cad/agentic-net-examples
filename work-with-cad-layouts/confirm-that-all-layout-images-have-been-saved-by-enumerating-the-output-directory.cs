using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

namespace CadLayoutExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file
                string sourcePath = "input.dwg";

                // Verify that the source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.WriteLine("Source CAD file not found: " + sourcePath);
                    return;
                }

                // Create output directory
                string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
                Directory.CreateDirectory(outputDir);

                // Load the CAD image
                using (CadImage cadImage = (CadImage)Image.Load(sourcePath))
                {
                    // Collect layout names
                    List<string> layoutNames = new List<string>();
                    foreach (var layoutEntry in cadImage.Layouts)
                    {
                        var layout = layoutEntry.Value;
                        if (!string.IsNullOrEmpty(layout.LayoutName))
                        {
                            layoutNames.Add(layout.LayoutName);
                        }
                    }

                    // If no layouts are found, inform the user
                    if (layoutNames.Count == 0)
                    {
                        Console.WriteLine("No layouts found in the CAD file.");
                        return;
                    }

                    // Export each layout to a PNG image
                    foreach (string layoutName in layoutNames)
                    {
                        // Configure rasterization options for the specific layout
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                        {
                            Layouts = new string[] { layoutName },
                            PageWidth = 1600f,
                            PageHeight = 1200f
                            // BackgroundColor omitted to avoid System.Drawing dependency
                        };

                        // Configure PNG save options
                        PngOptions pngOptions = new PngOptions
                        {
                            VectorRasterizationOptions = rasterOptions
                        };

                        // Build output file path
                        string outputPath = Path.Combine(outputDir, layoutName + ".png");

                        // Save the layout image
                        cadImage.Save(outputPath, pngOptions);
                    }
                }

                // Enumerate the output directory to confirm saved images
                string[] savedFiles = Directory.GetFiles(outputDir);
                Console.WriteLine("Saved layout images:");
                foreach (string filePath in savedFiles)
                {
                    Console.WriteLine(filePath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}