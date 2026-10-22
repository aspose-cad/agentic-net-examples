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
            string inputPath = "input.dwg";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDir = "output";
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, "modified.dwg");

            using (Image cadImage = Image.Load(inputPath))
            {
                // Place for modifications to the drawing.
                // Example (if supported): cadImage.Metadata["CustomProperty"] = "Modified";

                DwgOptions dwgOptions = new DwgOptions();
                // Example: set DWG version if needed.
                // dwgOptions.Version = Aspose.CAD.FileFormats.Cad.CadConsts.CadVersion.R2018;

                cadImage.Save(outputPath, dwgOptions);
            }

            Console.WriteLine($"Modified drawing saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}