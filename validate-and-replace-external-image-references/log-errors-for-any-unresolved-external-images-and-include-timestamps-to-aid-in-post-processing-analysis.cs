using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Define input and output file paths
        string inputPath = "input.dwg";
        string outputPath = "output.png";

        // Verify that the input file exists
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Error: Input file not found at '{inputPath}'.");
            return;
        }

        try
        {
            // Load the CAD file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Set up rasterization options (e.g., page size, layout)
                CadRasterizationOptions rasterizationOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    // Specify layout names if needed; here we use the default model layout
                    Layouts = new string[] { "Model" }
                };

                // Configure PNG export options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterizationOptions
                };

                // Save the CAD drawing as a PNG image
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Successfully saved PNG to '{outputPath}'.");
            }
        }
        catch (Exception ex)
        {
            // Log any errors that occur during processing
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}