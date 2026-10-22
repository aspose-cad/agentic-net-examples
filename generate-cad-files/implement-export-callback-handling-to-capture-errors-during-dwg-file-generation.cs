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
            string outputDir = "output";
            string outputPath = Path.Combine(outputDir, "output.dwg");

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            Directory.CreateDirectory(outputDir);

            using (Image cadImage = Image.Load(inputPath))
            {
                DwgOptions dwgOptions = new DwgOptions();
                // ExportCallback is not available in the current API.
                // Errors during export will be captured by the surrounding try-catch.

                cadImage.Save(outputPath, dwgOptions);
            }

            Console.WriteLine("DWG file saved successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Exception: {ex.Message}");
        }
    }
}