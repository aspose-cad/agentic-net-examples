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
            // Input folder containing CAD files (DWG, DXF, etc.)
            string inputFolder = "InputCadFiles";
            // Output folder for rendered PNG images
            string outputFolder = "OutputImages";

            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Get all CAD files in the input folder (common extensions)
            string[] cadFiles = Directory.GetFiles(inputFolder, "*.*", SearchOption.TopDirectoryOnly);
            foreach (string cadFilePath in cadFiles)
            {
                string extension = Path.GetExtension(cadFilePath).ToLowerInvariant();
                if (extension != ".dwg" && extension != ".dxf" && extension != ".dgn")
                {
                    continue; // Skip non‑CAD files
                }

                // Load the CAD file
                using (Image cadImage = Image.Load(cadFilePath))
                {
                    // Configure rasterization options
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,   // Desired output width
                        PageHeight = 1200,  // Desired output height
                        // PenColor and PenWidth are not available in the current API.
                        // Additional visual tweaks can be done via LineScale if needed.
                        LineScale = 1.0f
                    };

                    // Set up PNG export options and attach rasterization settings
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Build output file path
                    string outputFileName = Path.GetFileNameWithoutExtension(cadFilePath) + ".png";
                    string outputPath = Path.Combine(outputFolder, outputFileName);

                    // Save the rendered image
                    cadImage.Save(outputPath, pngOptions);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}