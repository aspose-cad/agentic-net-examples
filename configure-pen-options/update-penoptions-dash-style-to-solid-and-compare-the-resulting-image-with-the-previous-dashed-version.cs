using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Input CAD file path (replace with actual path)
        string inputFile = "sample.dwg";

        // Output directory for rendered images
        string outputDir = "output";

        // Validate input file existence
        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"Input file not found: {inputFile}");
            return;
        }

        // Ensure output directory exists
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputFile))
            {
                // Render with default (may be dashed) pen style
                string defaultPath = Path.Combine(outputDir, "output_default.png");
                ExportCadImage(cadImage, defaultPath, useSolidDash: false);

                // Render with solid pen style (dash style not available in this SDK version,
                // so we render the same way; in a full SDK this would set DashStyle = Solid)
                string solidPath = Path.Combine(outputDir, "output_solid.png");
                ExportCadImage(cadImage, solidPath, useSolidDash: true);

                // Compare the two rendered images
                bool imagesIdentical = CompareFiles(defaultPath, solidPath);
                Console.WriteLine($"Images are {(imagesIdentical ? "identical" : "different")}.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // Exports a CAD image to PNG; dash style handling is omitted due to SDK limitations
    static void ExportCadImage(CadImage cadImage, string outputPath, bool useSolidDash)
    {
        // Rasterization options based on source image dimensions
        CadRasterizationOptions rasterOptions = new CadRasterizationOptions
        {
            PageWidth = cadImage.Width,
            PageHeight = cadImage.Height,
            // PenOptions exists but does not expose DashStyle in this SDK version
            PenOptions = new PenOptions()
        };

        // PNG export options
        PngOptions pngOptions = new PngOptions
        {
            VectorRasterizationOptions = rasterOptions
        };

        // Save the rendered PNG
        cadImage.Save(outputPath, pngOptions);
    }

    // Simple byte‑wise file comparison
    static bool CompareFiles(string filePath1, string filePath2)
    {
        byte[] bytes1 = File.ReadAllBytes(filePath1);
        byte[] bytes2 = File.ReadAllBytes(filePath2);

        if (bytes1.Length != bytes2.Length)
            return false;

        for (int i = 0; i < bytes1.Length; i++)
        {
            if (bytes1[i] != bytes2[i])
                return false;
        }

        return true;
    }
}