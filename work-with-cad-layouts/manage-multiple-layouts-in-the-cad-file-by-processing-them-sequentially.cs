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
        try
        {
            // Input CAD file path
            string inputPath = "input.dwg";
            // Output directory for rendered layouts
            string outputDir = "output";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the CAD file
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to load CAD image.");
                    return;
                }

                // Iterate through each layout in the CAD file
                foreach (CadLayout layout in cadImage.Layouts.Values)
                {
                    // Prepare rasterization options for the current layout
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Render only the current layout
                        Layouts = new string[] { layout.LayoutName },
                        PageWidth = 1600,
                        PageHeight = 1200
                    };

                    // Prepare PNG export options
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Build output file name
                    string safeLayoutName = layout.LayoutName.Replace(Path.GetInvalidFileNameChars(), '_');
                    string outputPath = Path.Combine(outputDir, $"{safeLayoutName}.png");

                    // Save the rendered layout to PNG
                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"Rendered layout '{layout.LayoutName}' to {outputPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

// Extension method to replace invalid filename characters
static class StringExtensions
{
    public static string Replace(this string source, char[] chars, char replacement)
    {
        foreach (char c in chars)
        {
            source = source.Replace(c, replacement);
        }
        return source;
    }
}