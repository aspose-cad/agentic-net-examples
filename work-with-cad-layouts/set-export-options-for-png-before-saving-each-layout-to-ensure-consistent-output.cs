using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder input CAD file path
                string inputPath = "sample.dwg";

                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Create output directory
                string outputDir = "Output";
                Directory.CreateDirectory(outputDir);

                // Load the CAD drawing
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Define layout names to export (adjust as needed for the source file)
                    string[] layoutNames = new string[] { "Model", "Layout1" };

                    foreach (string layoutName in layoutNames)
                    {
                        // Configure rasterization options for the specific layout
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                        {
                            Layouts = new string[] { layoutName },
                            PageWidth = 1600,
                            PageHeight = 1200
                        };

                        // Configure PNG export options
                        PngOptions pngOptions = new PngOptions
                        {
                            VectorRasterizationOptions = rasterOptions
                        };

                        // Build output file path
                        string outputPath = Path.Combine(
                            outputDir,
                            $"{Path.GetFileNameWithoutExtension(inputPath)}_{layoutName}.png");

                        // Save the layout as PNG
                        cadImage.Save(outputPath, pngOptions);
                        Console.WriteLine($"Saved layout '{layoutName}' to: {outputPath}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}