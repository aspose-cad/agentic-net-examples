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
            // Define input and output paths
            string inputPath = "input.dwg";
            string outputDirectory = "Output";
            string outputPath = Path.Combine(outputDirectory, "exported.dwg");

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists
            Directory.CreateDirectory(outputDirectory);

            // Load the CAD image
            using (Image cadImage = Image.Load(inputPath))
            {
                // Prepare DWG export options
                DwgOptions dwgOptions = new DwgOptions();

                // Perform the export inside a try/catch to capture any exceptions
                try
                {
                    cadImage.Save(outputPath, dwgOptions);
                    Console.WriteLine($"DWG file successfully exported to: {outputPath}");
                }
                catch (Exception exportException)
                {
                    // Capture and report export errors
                    Console.Error.WriteLine($"Error during DWG export: {exportException.Message}");
                }
            }
        }
        catch (Exception unexpectedException)
        {
            // Capture any unexpected errors in the overall process
            Console.Error.WriteLine($"Unexpected error: {unexpectedException.Message}");
        }
    }
}