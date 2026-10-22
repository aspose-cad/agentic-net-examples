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
            // Define input OBJ file and output PNG file paths
            string inputPath = "input.obj";
            string outputPath = "output.png";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the OBJ file using Aspose.CAD
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure PNG export options
                PngOptions pngOptions = new PngOptions();
                // Example: set resolution if needed
                // pngOptions.Resolution = 300;

                // Save the loaded CAD image as PNG
                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine($"Conversion completed successfully: {outputPath}");
        }
        catch (Exception ex)
        {
            // Output any errors without crashing the application
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}