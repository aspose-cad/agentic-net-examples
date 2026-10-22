using System;
using System.IO;
using System.Diagnostics;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder paths – replace with actual directories.
            string inputDirectory = @"C:\Cad\InputStl";
            string outputDirectory = @"C:\Cad\OutputPng";

            // Verify that the input directory exists.
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
                return;
            }

            // Ensure the output directory exists.
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Get all STL files in the input directory.
            string[] stlFiles = Directory.GetFiles(inputDirectory, "*.stl");

            // Limit to the first 100 files if more are present.
            int filesToProcess = Math.Min(100, stlFiles.Length);
            if (filesToProcess == 0)
            {
                Console.Error.WriteLine("No STL files found to process.");
                return;
            }

            // Configure PNG export options.
            PngOptions pngOptions = new PngOptions
            {
                // Set rasterization options if needed (e.g., page size).
                VectorRasterizationOptions = new CadRasterizationOptions()
                {
                    // Example: define a default page size.
                    PageWidth = 1000,
                    PageHeight = 1000
                }
            };

            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < filesToProcess; i++)
            {
                string stlPath = stlFiles[i];
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(stlPath);
                string pngPath = Path.Combine(outputDirectory, fileNameWithoutExt + ".png");

                // Load the STL file.
                using (Image cadImage = Image.Load(stlPath))
                {
                    // Save as PNG using the configured options.
                    cadImage.Save(pngPath, pngOptions);
                }
            }

            stopwatch.Stop();

            Console.WriteLine($"Processed {filesToProcess} STL files in {stopwatch.Elapsed.TotalSeconds:F2} seconds.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}