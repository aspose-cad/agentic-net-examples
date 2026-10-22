using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source CAD file
            string sourcePath = "input.dwg";
            // Directory where exported layer images will be saved
            string outputDirectory = "output";

            // Verify source file exists
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            // Load the CAD file
            using (Image cadImage = Image.Load(sourcePath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cad = (CadImage)cadImage;

                // Ensure the output directory exists
                Directory.CreateDirectory(outputDirectory);

                // Iterate through all layer records
                foreach (CadLayerTable layerRecord in cad.Layers)
                {
                    // Example: select layers whose names start with "Export_"
                    if (layerRecord.Name != null && layerRecord.Name.StartsWith("Export_"))
                    {
                        // Configure rasterization to render only the selected layer
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                        {
                            Layers = new[] { layerRecord.Name },
                            PageWidth = 1600,
                            PageHeight = 1200
                        };

                        // Set up PNG export options
                        PngOptions pngOptions = new PngOptions
                        {
                            VectorRasterizationOptions = rasterOptions
                        };

                        // Build the output file path
                        string outputPath = Path.Combine(outputDirectory, layerRecord.Name + ".png");

                        // Export the selected layer to PNG
                        cadImage.Save(outputPath, pngOptions);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}