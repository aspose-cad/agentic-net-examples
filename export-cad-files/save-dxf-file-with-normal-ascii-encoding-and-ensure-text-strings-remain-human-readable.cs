using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class Program
{
    static void Main()
    {
        try
        {
            // Define input DXF file path
            string inputPath = "input.dxf";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare output directory and file path
            string outputDir = "output";
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, "output_ascii.dxf");

            // Load the DXF file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure save options with explicit ASCII format
                DxfOptions saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Ascii
                };

                // Save the DXF file using the specified options
                cadImage.Save(outputPath, saveOptions);
            }

            Console.WriteLine($"File saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}