using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Path to the source CAD file
        string inputPath = "input.dxf";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Load the CAD drawing
        using (Image cadImage = Image.Load(inputPath))
        {
            // Define output path and options (canvas size is omitted)
            string outputPath = "output.png";
            var pngOptions = new PngOptions();

            try
            {
                // Attempt to save the image without specifying canvas size
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine("Image saved successfully.");
            }
            // Aspose.CAD does not expose an ImageSaveException type; catch generic Exception instead
            catch (Exception ex)
            {
                Console.WriteLine("Error saving image: " + ex.Message);
            }
        }
    }
}