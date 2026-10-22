using System;
using System.IO;
using System.Threading.Tasks;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder input CAD files
            string[] inputFiles = new string[]
            {
                "input1.dwg",
                "input2.dwg",
                "input3.dxf"
            };

            // Placeholder output directory
            string outputDirectory = "output";

            // Verify that the output directory exists or create it
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Process each file concurrently
            Parallel.ForEach(inputFiles, (inputPath) =>
            {
                try
                {
                    // Verify input file exists
                    if (!File.Exists(inputPath))
                    {
                        Console.Error.WriteLine($"Input file not found: {inputPath}");
                        return;
                    }

                    // Load the CAD image
                    using (Image cadImage = Image.Load(inputPath))
                    {
                        // Prepare JPEG save options
                        JpegOptions jpegOptions = new JpegOptions
                        {
                            // You can set quality here if needed, e.g., Quality = 90
                        };

                        // Build output file path with .jpg extension
                        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(inputPath);
                        string outputPath = Path.Combine(outputDirectory, fileNameWithoutExt + ".jpg");

                        // Save the image as JPEG
                        cadImage.Save(outputPath, jpegOptions);
                        Console.WriteLine($"Converted '{inputPath}' to JPEG successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file '{inputPath}': {ex.Message}");
                }
            });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}