using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Ensure output directory exists
            string? outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the DWG file
            using (Image image = Image.Load(inputPath))
            {
                if (image is not CadImage cadImage)
                {
                    Console.Error.WriteLine("Loaded image is not a CadImage.");
                    return;
                }

                // NOTE: The current Aspose.CAD API does not provide access to embedded DGN data
                // or a Metadata property for CadImage. Therefore, modifying DGN data or metadata
                // inside a DWG file is not supported.

                Console.WriteLine("Access to DGN data and metadata modification is not available in this SDK version.");

                // Save the (unchanged) drawing back to DWG format
                cadImage.Save(outputPath, null);
            }

            Console.WriteLine($"File saved successfully to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}