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
            string inputPath = "sample.dwf";
            string outputPath = "output.png";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image dwfImage = Image.Load(inputPath))
            {
                // Display unit type of the drawing
                Console.WriteLine($"Unit type: {dwfImage.UnitType}");

                // Save the drawing as PNG
                PngOptions pngOptions = new PngOptions();
                dwfImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Saved PNG to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}