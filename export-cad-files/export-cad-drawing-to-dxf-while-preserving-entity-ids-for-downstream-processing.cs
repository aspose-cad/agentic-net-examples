using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output file paths
            string inputPath = "input.dwg";
            string outputPath = "output.dxf";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure DXF export options
                var dxfOptions = new DxfOptions
                {
                    // Use Convert mode to retain entity information as much as possible
                    OutputMode = CadOutputMode.Convert
                };

                // Save the drawing as DXF
                cadImage.Save(outputPath, dxfOptions);
            }

            Console.WriteLine($"CAD drawing exported to DXF successfully: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}