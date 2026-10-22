using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder input and output directories
            string inputDirectory = "InputStlFiles";
            string outputDirectory = "OutputPngFiles";

            // Verify input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
                return;
            }

            // Ensure output directory exists
            Directory.CreateDirectory(outputDirectory);

            // Get all STL files in the input directory
            string[] stlFiles = Directory.GetFiles(inputDirectory, "*.stl", SearchOption.TopDirectoryOnly);
            if (stlFiles.Length == 0)
            {
                Console.Error.WriteLine($"No STL files found in: {inputDirectory}");
                return;
            }

            foreach (string stlFilePath in stlFiles)
            {
                // Load the STL file using Aspose.CAD
                using (Image cadImage = Image.Load(stlFilePath))
                {
                    // Configure rasterization options (e.g., page size)
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Example settings; adjust as needed
                        PageWidth = 800,
                        PageHeight = 600,
                        // Optional: set background color, layout, etc.
                        // BackgroundColor = System.Drawing.Color.White,
                        // Layouts = new[] { "Model" }
                    };

                    // Set up PNG export options with the rasterization settings
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Determine output file path
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(stlFilePath);
                    string outputFilePath = Path.Combine(outputDirectory, fileNameWithoutExt + ".png");

                    // Save the rasterized image as PNG
                    cadImage.Save(outputFilePath, pngOptions);
                    Console.WriteLine($"Converted '{stlFilePath}' to '{outputFilePath}'.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}