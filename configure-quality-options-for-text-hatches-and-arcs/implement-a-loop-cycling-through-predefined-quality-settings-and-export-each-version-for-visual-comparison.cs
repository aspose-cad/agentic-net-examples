using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadQualityExportSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define input CAD file path (placeholder)
            string inputPath = "input.dwg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Define output directory and ensure it exists
            string outputDir = "Output";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Predefined JPEG quality settings for comparison
            int[] qualitySettings = new int[] { 100, 80, 60, 40, 20 };

            try
            {
                // Load the CAD image
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Loop through each quality setting and export
                    foreach (int quality in qualitySettings)
                    {
                        // Configure JPEG export options with the current quality
                        JpegOptions jpegOptions = new JpegOptions();
                        jpegOptions.Quality = quality;

                        // Build output file name
                        string outputPath = Path.Combine(outputDir, $"output_quality_{quality}.jpg");

                        // Save the CAD image as JPEG with the specified quality
                        cadImage.Save(outputPath, jpegOptions);

                        Console.WriteLine($"Saved: {outputPath} (Quality={quality})");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}