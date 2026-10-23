using Aspose.CAD.FileFormats.Cad;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Dwf;
using Aspose.CAD.FileFormats.Dwf.Whip.Objects;

class Program
{
    static void Main()
    {
        // Input DWF file path (replace with actual path)
        string inputFilePath = @"C:\CAD\sample.dwf";

        // Output directory for PNG images
        string outputDirectory = @"C:\CAD\ExportedLayers";

        try
        {
            // Verify input file exists
            if (!File.Exists(inputFilePath))
                throw new FileNotFoundException("Input file not found.", inputFilePath);

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            // Load the DWF image
            using (var image = Aspose.CAD.Image.Load(inputFilePath))
            {
                if (image is DwfImage dwfImage)
                {
                    // Collect layer names
                    var layerNames = new List<string>();
                    foreach (DwfWhipLayer layer in dwfImage.Layers)
                    {
                        if (layer?.Name?.AsciiString != null)
                            layerNames.Add(layer.Name.AsciiString);
                    }

                    if (layerNames.Count == 0)
                    {
                        Console.WriteLine("No layers found in the DWF file.");
                        return;
                    }

                    foreach (var layerName in layerNames)
                    {
                        // Set rasterization options for the specific layer
                        var rasterOptions = new CadRasterizationOptions
                        {
                            Layers = new[] { layerName },
                            UnitType = UnitType.Centimenter,
                            LineScale = 1.0f,
                            BackgroundColor = Aspose.CAD.Color.White,
                            DrawColor = Aspose.CAD.Color.Red,
                            DrawType = CadDrawTypeMode.UseDrawColor
                        };

                        // PNG export options
                        var pngOptions = new PngOptions
                        {
                            VectorRasterizationOptions = rasterOptions
                        };

                        // Build output file path
                        string outputFilePath = Path.Combine(outputDirectory, $"{layerName}.png");

                        // Export the current layer as PNG
                        dwfImage.Save(outputFilePath, pngOptions);
                    }

                    Console.WriteLine("Export completed successfully.");
                }
                else
                {
                    Console.WriteLine("The input file is not a DWF format; layer extraction is not supported.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}