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
            // Placeholder paths – replace with actual file locations
            string inputPath = "sample.plt";
            string outputPath = "output.png";

            // Verify that the source PLT file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the PLT file using Aspose.CAD.Image
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure PNG export options (optional customizations can be added)
                PngOptions pngOptions = new PngOptions();

                // Save the loaded CAD image as a PNG raster image
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"File saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            // Print concise error information without crashing
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}