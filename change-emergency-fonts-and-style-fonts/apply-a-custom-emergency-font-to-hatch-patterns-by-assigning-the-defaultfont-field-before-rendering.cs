using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace AsposeCadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder paths – replace with actual file locations.
            string inputPath = "input.dwg";
            string outputPath = "output.png";

            try
            {
                // Verify that the input file exists.
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD file.
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Set up rasterization options for rendering.
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Example rendering size – adjust as needed.
                        PageWidth = 1600,
                        PageHeight = 1200,
                        // Optional: set background color, layouts, layers, etc.
                        // BackgroundColor = System.Drawing.Color.White
                    };

                    // Configure PNG output options.
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the rendered image.
                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"CAD file successfully saved to: {outputPath}");
                }
            }
            catch (Exception ex)
            {
                // Print concise error information.
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
