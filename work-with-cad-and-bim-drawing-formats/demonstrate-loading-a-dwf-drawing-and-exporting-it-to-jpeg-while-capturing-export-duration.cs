using System;
using System.IO;
using System.Diagnostics;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input and output file paths
            string inputPath = "input.dwf";
            string outputPath = "output.jpg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the DWF drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Prepare JPEG export options
                JpegOptions jpegOptions = new JpegOptions();

                // Measure export duration
                Stopwatch stopwatch = Stopwatch.StartNew();
                cadImage.Save(outputPath, jpegOptions);
                stopwatch.Stop();

                Console.WriteLine($"Export completed in {stopwatch.ElapsedMilliseconds} ms.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}