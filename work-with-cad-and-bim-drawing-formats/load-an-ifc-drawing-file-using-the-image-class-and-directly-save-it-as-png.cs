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
            // Define input and output file paths
            string inputPath = "input.ifc";
            string outputPath = "output.png";

            // Verify that the input IFC file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the IFC drawing using Aspose.CAD.Image
            using (Image cadImage = Image.Load(inputPath))
            {
                // Prepare PNG export options
                PngOptions pngOptions = new PngOptions();

                // Save the drawing as a PNG file
                cadImage.Save(outputPath, pngOptions);
            }

            Console.WriteLine($"File saved to {outputPath}");
        }
        catch (Exception ex)
        {
            // Print any errors without crashing the application
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}