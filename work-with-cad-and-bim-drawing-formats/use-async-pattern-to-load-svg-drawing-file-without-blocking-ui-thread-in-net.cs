using System;
using System.IO;
using System.Threading.Tasks;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace AsposeCadAsyncExample
{
    class Program
    {
        // Asynchronously loads a CAD image (SVG) without blocking the calling thread.
        private static async Task<CadImage> LoadSvgAsync(string filePath)
        {
            // Image.Load is synchronous; wrap it in Task.Run to offload to a background thread.
            return await Task.Run(() => (CadImage)Image.Load(filePath));
        }

        static async Task Main(string[] args)
        {
            // Path to the source SVG file.
            string inputPath = "input.svg";

            // Guard against missing input file.
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the SVG file asynchronously.
            CadImage cadImage = await LoadSvgAsync(inputPath);

            try
            {
                // Configure rasterization options.
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter, // Correct enum spelling.
                    LineScale = 1.0f,
                    PageWidth = 800,
                    PageHeight = 600,
                    AutomaticLayoutsScaling = true,
                    Margins = new Margins
                    {
                        Left = 10,
                        Right = 10,
                        Top = 10,
                        Bottom = 10
                    }
                };

                // Prepare PNG output options and assign rasterization settings.
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Define output path and ensure its directory exists.
                string outputPath = "output.png";
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the rendered image to PNG.
                cadImage.Save(outputPath, pngOptions);
            }
            finally
            {
                // Ensure the image resources are released.
                cadImage.Dispose();
            }

            Console.WriteLine("SVG file loaded asynchronously and saved as PNG.");
        }
    }
}