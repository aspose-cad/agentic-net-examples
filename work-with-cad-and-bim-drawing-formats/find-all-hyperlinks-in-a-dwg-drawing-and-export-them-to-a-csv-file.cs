using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputCsv = "hyperlinks.csv";

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists (if a directory is specified)
            string outputDir = Path.GetDirectoryName(outputCsv);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("The loaded file is not a CAD image.");
                    return;
                }

                // Aspose.CAD does not expose hyperlinks in the current API.
                Console.Error.WriteLine("Hyperlink extraction is not supported by the Aspose.CAD API.");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}