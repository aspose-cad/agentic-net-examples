using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Placeholder paths – replace with actual file locations
        string inputPath = @"C:\Input\sample.dwg";
        string outputPath = @"C:\Output\sample.png";

        try
        {
            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file using Aspose.CAD.Image.Load
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options (e.g., page size, layout)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    // Render the "Model" layout; adjust as needed
                    Layouts = new string[] { "Model" }
                };

                // Set up PNG export options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the CAD drawing as a PNG image
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"CAD file successfully converted to PNG: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}