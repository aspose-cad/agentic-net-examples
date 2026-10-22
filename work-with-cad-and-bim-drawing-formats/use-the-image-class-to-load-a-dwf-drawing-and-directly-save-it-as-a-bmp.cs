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
            // Define input DWF file and output BMP file paths
            string inputPath = "input.dwf";
            string outputPath = "output.bmp";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the DWF drawing using Aspose.CAD.Image.Load
            using (Image dwfImage = Image.Load(inputPath))
            {
                // Create BMP save options
                BmpOptions bmpOptions = new BmpOptions();

                // Save the drawing directly as BMP
                dwfImage.Save(outputPath, bmpOptions);
            }

            Console.WriteLine($"Successfully saved BMP to {outputPath}");
        }
        catch (Exception ex)
        {
            // Print any errors without crashing the application
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}