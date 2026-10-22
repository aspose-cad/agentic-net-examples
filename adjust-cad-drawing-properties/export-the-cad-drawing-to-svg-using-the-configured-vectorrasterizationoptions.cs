using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.svg";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            try
            {
                Directory.CreateDirectory(outputDir);
            }
            catch (Exception dirEx)
            {
                Console.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }
        }

        try
        {
            using (Image cadImage = Image.Load(inputPath))
            {
                // Create SVG export options
                SvgOptions svgOptions = new SvgOptions();

                // Configure vector rasterization options for the SVG output
                CadRasterizationOptions vectorOptions = new CadRasterizationOptions
                {
                    PageWidth = 800,   // Width of the output page in pixels
                    PageHeight = 600   // Height of the output page in pixels
                    // BackgroundColor omitted to avoid System.Drawing usage
                };

                // Assign the vector rasterization options to the SVG options
                svgOptions.VectorRasterizationOptions = vectorOptions;

                // Save the CAD drawing as an SVG file using the configured options
                try
                {
                    cadImage.Save(outputPath, svgOptions);
                    Console.WriteLine($"SVG file saved successfully to {outputPath}");
                }
                catch (Exception saveEx)
                {
                    Console.WriteLine($"Error saving SVG file: {saveEx.Message}");
                }
            }
        }
        catch (Exception loadEx)
        {
            Console.WriteLine($"Error loading CAD file: {loadEx.Message}");
        }
    }
}
