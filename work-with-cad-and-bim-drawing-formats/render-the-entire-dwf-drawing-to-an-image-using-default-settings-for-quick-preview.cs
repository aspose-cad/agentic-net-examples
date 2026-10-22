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
            // Define input DWF file path (replace with actual path)
            string inputFilePath = "input.dwf";

            // Verify that the input file exists
            if (!File.Exists(inputFilePath))
            {
                Console.Error.WriteLine($"Input file not found: {inputFilePath}");
                return;
            }

            // Load the DWF drawing
            using (Image cadImage = Image.Load(inputFilePath))
            {
                // Prepare output directory
                string outputDir = "output";
                Directory.CreateDirectory(outputDir);

                // Define output image path
                string outputFilePath = Path.Combine(outputDir, "preview.png");

                // Set PNG save options (default settings)
                PngOptions pngOptions = new PngOptions();

                // Save the rendered image
                cadImage.Save(outputFilePath, pngOptions);

                Console.WriteLine($"Preview image saved to: {outputFilePath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}