using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Placeholder paths for input CAD file and output image
        string inputPath = "input.dwg";
        string outputPath = "output.png";

        // Verify that the input file exists before proceeding
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the CAD drawing; Image implements IDisposable
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options for the export
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Example settings (adjust as needed)
                    // BackgroundColor = System.Drawing.Color.White,
                    // PageWidth = 1600,
                    // PageHeight = 1200,
                    // Layouts = new string[] { "Model" }, // optional layout selection
                    // Layers = new string[] { "0" }       // optional layer selection
                };

                // Set up PNG output options and attach rasterization settings
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Export the CAD drawing to PNG
                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine($"Export completed successfully: {outputPath}");
        }
        catch (Exception ex)
        {
            // Print concise error information without crashing the application
            Console.Error.WriteLine($"Error during export: {ex.Message}");
        }
    }
}