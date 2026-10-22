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
            // Input CAD file path (replace with an actual file path)
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Create output directory
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
            Directory.CreateDirectory(outputDir);

            // Load CAD image
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access layouts
                CadImage cadImage = (CadImage)image;

                // Iterate through each layout in the CAD drawing
                foreach (CadLayout layout in cadImage.Layouts.Values)
                {
                    string layoutName = layout.LayoutName;

                    // Set rasterization options for the specific layout
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        Layouts = new[] { layoutName },
                        PageWidth = 1600,
                        PageHeight = 1200
                    };

                    // Set PNG save options
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Build output file path
                    string outputFileName = $"{Path.GetFileNameWithoutExtension(inputPath)}_{layoutName}.png";
                    string outputPath = Path.Combine(outputDir, outputFileName);

                    // Save the layout as an image
                    cadImage.Save(outputPath, pngOptions);

                    // Verify that the file was created
                    bool fileExists = File.Exists(outputPath);
                    Console.WriteLine($"Layout '{layoutName}' saved to '{outputPath}': {(fileExists ? "Success" : "Failed")}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}