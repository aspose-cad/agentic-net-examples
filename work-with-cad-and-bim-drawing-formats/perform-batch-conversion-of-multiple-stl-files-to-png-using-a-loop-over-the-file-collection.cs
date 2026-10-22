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
            // Define input and output directories
            string inputFolder = "InputStlFiles";
            string outputFolder = "OutputPngFiles";

            // Verify input directory exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder does not exist: {inputFolder}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Get all STL files in the input directory
            string[] stlFiles = Directory.GetFiles(inputFolder, "*.stl", SearchOption.TopDirectoryOnly);
            if (stlFiles.Length == 0)
            {
                Console.WriteLine("No STL files found to convert.");
                return;
            }

            // Loop through each STL file and convert to PNG
            foreach (string stlPath in stlFiles)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(stlPath);
                string pngPath = Path.Combine(outputFolder, fileNameWithoutExtension + ".png");

                // Load the STL file
                using (Image cadImage = Image.Load(stlPath))
                {
                    // Prepare PNG save options
                    PngOptions pngOptions = new PngOptions();

                    // Optional: configure rasterization options if needed
                    // pngOptions.VectorRasterizationOptions = new CadRasterizationOptions
                    // {
                    //     PageWidth = 1000,
                    //     PageHeight = 1000,
                    //     Layouts = new[] { "Model" }
                    // };

                    // Save as PNG
                    cadImage.Save(pngPath, pngOptions);
                }

                Console.WriteLine($"Converted '{stlPath}' to '{pngPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}