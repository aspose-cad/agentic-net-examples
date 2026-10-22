using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadIntegrationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define input and output file paths
            string inputFilePath = "sample.dwg";
            string outputFilePath = "sample.png";

            // Guard: check input file exists
            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine($"Input file not found: {inputFilePath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputFilePath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Configure load options to ignore errors during loading
            LoadOptions loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            try
            {
                // Load the CAD drawing using Aspose.CAD.Image.Load
                using (Image cadImage = Image.Load(inputFilePath, loadOptions))
                {
                    // Prepare PNG export options
                    PngOptions pngOptions = new PngOptions();

                    // Export the loaded CAD drawing to PNG format
                    cadImage.Save(outputFilePath, pngOptions);
                }

                Console.WriteLine("CAD drawing loaded and exported successfully.");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during load or save
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}