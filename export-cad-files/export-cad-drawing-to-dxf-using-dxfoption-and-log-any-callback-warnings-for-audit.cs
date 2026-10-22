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
            string inputPath = "input.dwg";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDir = "output";
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, "output.dxf");

            using (Image cadImage = Image.Load(inputPath))
            {
                // LoadWarnings are not available in the current API version.
                var dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert // Preserve editable objects in DXF
                };

                cadImage.Save(outputPath, dxfOptions);
            }

            Console.WriteLine($"Export completed: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}