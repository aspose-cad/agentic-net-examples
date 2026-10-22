using System;
using System.IO;
using System.Threading.Tasks;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static async Task Main(string[] args)
    {
        // Path to the source STEP file
        string inputFile = "sample.stp";
        // Path for the rendered output image
        string outputFile = "output.png";

        // Verify input file exists
        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"Input file not found: {inputFile}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputFile);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        Image cadImage = null;
        try
        {
            // Load the STEP drawing asynchronously to avoid blocking the UI thread
            cadImage = await Task.Run(() => Image.Load(inputFile));

            // Configure PNG export options
            var pngOptions = new PngOptions();

            // Save the loaded CAD image as a PNG file
            cadImage.Save(outputFile, pngOptions);
            Console.WriteLine($"Image saved to {outputFile}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
        finally
        {
            // Ensure the CAD image resources are released
            cadImage?.Dispose();
        }
    }
}