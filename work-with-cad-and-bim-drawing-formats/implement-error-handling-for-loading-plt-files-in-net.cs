using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Placeholder path to the PLT file
        string inputPath = "sample.plt";

        // Verify that the input file exists
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load the PLT file using Aspose.CAD
            using (Image cadImage = Image.Load(inputPath))
            {
                // Output basic information about the loaded image
                Console.WriteLine($"Loaded PLT file. Dimensions: {cadImage.Width}x{cadImage.Height}");

                // Example: rasterize and save as PNG
                string outputPath = "output.png";
                PngOptions pngOptions = new PngOptions();
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Rasterized image saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            // Print concise error information without crashing
            Console.Error.WriteLine($"Error loading PLT file: {ex.Message}");
        }
    }
}