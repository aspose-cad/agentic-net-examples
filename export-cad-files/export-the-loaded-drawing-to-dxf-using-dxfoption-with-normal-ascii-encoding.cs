using Aspose.CAD.FileFormats.Cad.CadConsts;
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
            // Define input and output paths
            string inputPath = "input.dwg";
            string outputDirectory = "output";
            string outputPath = Path.Combine(outputDirectory, "output.dxf");

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure DXF export options with explicit ASCII format
                var dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Ascii
                };

                // Save the drawing as DXF
                cadImage.Save(outputPath, dxfOptions);
            }

            Console.WriteLine($"DXF file saved successfully to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}