using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Path to the source CAD file
        string inputPath = "input.dwg";

        // Directory for the exported raster image
        string outputDirectory = "output";

        // Full path for the exported PNG file
        string outputPath = Path.Combine(outputDirectory, "output.png");

        try
        {
            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the CAD drawing
            CadImage cadImage = (CadImage)Image.Load(inputPath);

            // Access and display layout names
            Console.WriteLine("Layouts:");
            foreach (var layout in cadImage.Layouts.Values)
            {
                Console.WriteLine($"- {layout.LayoutName}");
            }

            // Retrieve and display layer names
            Console.WriteLine("Layers:");
            foreach (CadLayerTable layer in cadImage.Layers)
            {
                Console.WriteLine($"- {layer.Name}");
            }

            // Configure rasterization options with Zoom = 1
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                Zoom = 1.0f
            };

            // Create PNG export options and assign rasterization settings
            PngOptions pngOptions = new PngOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Export the drawing to PNG
            cadImage.Save(outputPath, pngOptions);

            Console.WriteLine($"Raster image saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}