using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

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

                // Load the CAD file using Aspose.CAD.
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Configure rasterization options.
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Set desired output size.
                        PageWidth = 1600,
                        PageHeight = 1200,
                        // Optionally specify layout names (e.g., "Model").
                        Layouts = new string[] { "Model" }
                    };

                    // Set PNG export options.
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the rendered image to the output path.
                    cadImage.Save(outputPath, pngOptions);
                }

                Console.WriteLine($"CAD file successfully converted and saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                // Output concise error information.
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
