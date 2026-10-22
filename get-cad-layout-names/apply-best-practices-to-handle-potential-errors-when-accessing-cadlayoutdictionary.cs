using System;
using System.IO;
using System.Collections.Generic;
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
            // Input CAD file path (replace with actual file)
            string inputPath = "sample.dwg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file using Aspose.CAD
            using (Image image = Image.Load(inputPath))
            {
                // Cast the loaded image to CadImage to access CAD-specific members
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Loaded image is not a CAD image.");
                    return;
                }

                // Safely access the layout collection
                var layouts = cadImage.Layouts;
                if (layouts == null || layouts.Count == 0)
                {
                    Console.WriteLine("No layouts found in the CAD file.");
                    return;
                }

                // Iterate through each layout with error handling
                foreach (KeyValuePair<string, CadLayout> kvp in layouts)
                {
                    string layoutName = kvp.Key;
                    CadLayout layout = kvp.Value;

                    if (layout == null)
                    {
                        Console.WriteLine($"Layout '{layoutName}' is null, skipping.");
                        continue;
                    }

                    Console.WriteLine($"Processing layout '{layoutName}'");

                    // Define output file name for the current layout
                    string outputFileName = $"{Path.GetFileNameWithoutExtension(inputPath)}_{layoutName}.png";
                    string outputPath = Path.Combine(Directory.GetCurrentDirectory(), outputFileName);

                    // Ensure the output directory exists
                    string outputDir = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    // Configure rasterization options for the specific layout
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        Layouts = new[] { layoutName },
                        PageWidth = cadImage.Width,
                        PageHeight = cadImage.Height
                        // BackgroundColor omitted to avoid System.Drawing dependency
                    };

                    // Set PNG export options
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Export the layout to PNG
                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"Saved layout '{layoutName}' to '{outputPath}'.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}