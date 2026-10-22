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
        // Paths (replace with actual paths as needed)
        string inputPath = "input.dwg";
        string outputDir = "output";
        string outputPath = Path.Combine(outputDir, "result.png");

        try
        {
            // Validate input file
            if (!File.Exists(inputPath))
                throw new FileNotFoundException($"Input file not found: {inputPath}");

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Load CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Retrieve and display list of layers
                Console.WriteLine("Layers in the CAD file:");
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    Console.WriteLine($"- Name: {layer.Name}, ColorId: {layer.ColorId}, Flags: {layer.Flags}, PlotFlag: {layer.PlotFlag}");
                }

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,   // Set unit type
                    LineScale = 1.0f,                  // Default line scale
                    Zoom = 1.0f,                       // Border‑free zoom
                    Margins = new Margins
                    {
                        Left = 0,
                        Right = 0,
                        Top = 0,
                        Bottom = 0
                    }
                };

                // Prepare PNG export options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the image
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Image successfully saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}