using Aspose.CAD.FileFormats.Cad;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Path to the source IFC drawing
        string sourcePath = "input.ifc";

        // Path for the exported PNG image
        string outputPath = "output.png";

        // Verify source file exists
        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the IFC drawing as a CadImage
            using (Image cadImage = Image.Load(sourcePath))
            {
                // Note: Aspose.CAD PngOptions does not expose a writable BackgroundColor property.
                // Therefore, background color cannot be set directly via the API.
                // Export the CAD drawing to PNG using default settings.
                PngOptions pngOptions = new PngOptions();

                cadImage.Save(outputPath, pngOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}