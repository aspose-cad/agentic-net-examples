using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "sample.dwg";
        string outputPath = "output.dxf";

        try
        {
            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the CAD file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure DXF export options
                DxfOptions dxfOptions = new DxfOptions();
                // Preserve mode is not available; use Convert mode
                dxfOptions.OutputMode = CadOutputMode.Convert;

                // Save the image as DXF
                cadImage.Save(outputPath, dxfOptions);
            }

            Console.WriteLine($"File successfully saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}