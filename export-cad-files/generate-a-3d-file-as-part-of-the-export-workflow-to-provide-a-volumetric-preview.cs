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
            string outputPath = "output.stl";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image cadImage = Image.Load(inputPath))
            {
                StlOptions stlOptions = new StlOptions();
                // ExportMode property is not available in this version; default settings are used.
                cadImage.Save(outputPath, stlOptions);
            }

            Console.WriteLine($"STL file exported successfully to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}