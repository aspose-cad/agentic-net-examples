using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder file paths
            string inputPath = "input.dwg";
            string outputPath = "output.png";
            string fallbackFontPath = "fallback.ttf";

            // Validate input file existence
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                string outputDir = Path.GetDirectoryName(outputPath) ?? string.Empty;
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            try
            {
                // Load CAD image
                Image cadImage = Image.Load(inputPath);

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200
                    // Note: In the current Aspose.CAD version, the fallback emergency font
                    // can be set via the DefaultFont property if available.
                    // If not present, ensure the required fonts are installed on the system.
                };

                // Set up PNG export options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the rendered image
                try
                {
                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"Image saved successfully to {outputPath}");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Error saving image: {saveEx.Message}");
                }
                finally
                {
                    cadImage.Dispose();
                }
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Error loading CAD file: {loadEx.Message}");
            }
        }
    }
}
