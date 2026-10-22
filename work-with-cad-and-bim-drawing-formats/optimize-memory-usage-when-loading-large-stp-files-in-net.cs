using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for input STEP file and output PNG file
            string inputPath = "large_model.stp";
            string outputPath = "output.png";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Configure load options to ignore errors (helps with large files)
            LoadOptions loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            // Load the STEP file
            using (Image cadImage = Image.Load(inputPath, loadOptions))
            {
                // Set rasterization options to limit memory usage
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1000,   // limit width
                    PageHeight = 1000   // limit height
                    // No Layout property; omitted
                    // BackgroundColor omitted to avoid System.Drawing usage
                };

                // Prepare PNG export options with the rasterization settings
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the rendered image
                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}