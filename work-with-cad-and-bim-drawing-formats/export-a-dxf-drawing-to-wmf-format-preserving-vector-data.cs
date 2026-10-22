using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Path to the source DXF file
        string sourcePath = "input.dxf";

        // Verify source file exists
        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        // Path for the output WMF file
        string outputPath = "output.wmf";

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the DXF drawing
            using (Image cadImage = Image.Load(sourcePath))
            {
                // Configure WMF save options (vector data is preserved by default)
                WmfOptions wmfOptions = new WmfOptions();

                // Save the drawing as WMF
                cadImage.Save(outputPath, wmfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}