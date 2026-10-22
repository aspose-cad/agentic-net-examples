using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Determine input file path
            string inputPath = args.Length > 0 ? args[0] : "sample.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare output directory
            string outputDirectory = Path.Combine(Path.GetDirectoryName(inputPath) ?? "", "output");
            Directory.CreateDirectory(outputDirectory);

            // Load CAD file
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Get distinct layer names from entities
                var layerNames = cadImage.Entities
                    .Select(e => e.LayerName)
                    .Where(name => !string.IsNullOrEmpty(name))
                    .Distinct()
                    .ToList();

                if (!layerNames.Any())
                {
                    Console.WriteLine("No layers found in the CAD file.");
                    return;
                }

                foreach (string layerName in layerNames)
                {
                    // Prepare PNG save options with rasterization limited to the current layer
                    PngOptions pngOptions = new PngOptions();
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Use original image dimensions
                        PageWidth = image.Width,
                        PageHeight = image.Height,
                        // Render only the specific layer
                        Layers = new[] { layerName }
                    };
                    pngOptions.VectorRasterizationOptions = rasterOptions;

                    // Build output file path
                    string outputFileName = $"{Path.GetFileNameWithoutExtension(inputPath)}_{layerName}.png";
                    string outputPath = Path.Combine(outputDirectory, outputFileName);

                    // Save the layer as PNG
                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"Saved layer '{layerName}' to {outputPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}