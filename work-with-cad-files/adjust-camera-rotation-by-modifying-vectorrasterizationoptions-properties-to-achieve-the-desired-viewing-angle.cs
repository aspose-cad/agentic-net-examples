using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class AdjustCameraRotation
{
    static void Main(string[] args)
    {
        // Input CAD file path
        string inputPath = @"C:\CAD\sample.dwg";
        // Output PNG file path
        string outputPath = @"C:\CAD\output.png";

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Create PNG export options
                PngOptions pngOptions = new PngOptions();

                // NOTE: The current Aspose.CAD API does not expose a RotationAngle property
                // in VectorRasterizationOptions, so camera rotation cannot be adjusted via code.
                // The following demonstrates a basic export without rotation adjustments.

                // Save the rendered image
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Image saved successfully to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}