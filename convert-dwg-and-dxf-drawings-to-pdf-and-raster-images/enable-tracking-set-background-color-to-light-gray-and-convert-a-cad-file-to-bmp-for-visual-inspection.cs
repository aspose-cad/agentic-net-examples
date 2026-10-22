using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Input CAD file path
        string inputPath = "sample.dwg";

        // Output directory and file path
        string outputDir = "output";
        string outputPath = Path.Combine(outputDir, "output.bmp");

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        if (!Directory.Exists(outputDir))
            Directory.CreateDirectory(outputDir);

        try
        {
            // Load the CAD image
            using (Image cadImage = Image.Load(inputPath))
            {
                // Prepare BMP export options (default settings)
                var bmpOptions = new BmpOptions();

                // Save as BMP
                cadImage.Save(outputPath, bmpOptions);
                Console.WriteLine($"CAD file successfully converted to BMP at: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}