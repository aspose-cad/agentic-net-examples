using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class CadLayoutExporter
{
    static void Main(string[] args)
    {
        // Validate arguments
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: CadLayoutExporter <input-cad-file> <output-directory>");
            return;
        }

        string inputPath = args[0];
        string outputDir = args[1];

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        if (!Directory.Exists(outputDir))
        {
            try
            {
                Directory.CreateDirectory(outputDir);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }
        }

        try
        {
            // Load the CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Iterate through all layouts
                int layoutIndex = 0;
                foreach (var layoutKvp in cadImage.Layouts)
                {
                    string layoutName = layoutKvp.Value.LayoutName ?? $"Layout{layoutIndex}";
                    // Sanitize layout name for file system
                    string safeLayoutName = GetSafeFileName(layoutName);

                    // Build unique output file name
                    string baseFileName = Path.GetFileNameWithoutExtension(inputPath);
                    string outputFileName = $"{baseFileName}_{safeLayoutName}_{layoutIndex}.png";
                    string outputPath = Path.Combine(outputDir, outputFileName);

                    // Configure rasterization options to render only the current layout
                    var rasterOptions = new CadRasterizationOptions
                    {
                        // Render only the specific layout
                        Layouts = new[] { layoutName },
                        // Optional: set page size or scaling as needed
                        AutomaticLayoutsScaling = true
                    };

                    // Set PNG export options
                    var pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the layout as PNG
                    cadImage.Save(outputPath, pngOptions);

                    Console.WriteLine($"Saved layout '{layoutName}' to '{outputPath}'");
                    layoutIndex++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }

    // Helper to replace invalid filename characters
    private static string GetSafeFileName(string name)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        foreach (var ch in invalidChars)
        {
            name = name.Replace(ch, '_');
        }
        // Trim any leading/trailing whitespace
        return name.Trim();
    }
}