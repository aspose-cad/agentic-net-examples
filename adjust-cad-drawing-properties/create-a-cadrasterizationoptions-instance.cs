using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadRasterizationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input and output file paths
            string inputPath = "input.dwg";
            string outputPath = "output.png";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Load CAD image with error handling
            try
            {
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Create CadRasterizationOptions instance
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    rasterOptions.PageWidth = 1600;
                    rasterOptions.PageHeight = 1200;
                    string[] layouts = new string[] { "Model" };
                    rasterOptions.Layouts = layouts;

                    // Set up PNG save options
                    PngOptions pngOptions = new PngOptions();
                    pngOptions.VectorRasterizationOptions = rasterOptions;

                    // Save the rasterized image with error handling
                    try
                    {
                        cadImage.Save(outputPath, pngOptions);
                        Console.WriteLine($"CAD file successfully rasterized to PNG: {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error during saving PNG: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading CAD file: {ex.Message}");
            }
        }
    }
}
