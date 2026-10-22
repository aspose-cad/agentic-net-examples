using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.png";
        const string viewportName = "VIEW1"; // replace with the actual viewport name

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file \"{inputPath}\" not found.");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to work with CAD‑specific features
                CadImage cadImage = (CadImage)image;

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Prevent automatic scaling of layouts (including viewports)
                    AutomaticLayoutsScaling = false,
                    // Set the output image size (adjust as needed to match the viewport dimensions)
                    PageWidth = 1200,
                    PageHeight = 800,
                    // Specify the layout (or viewport) to render
                    Layouts = new[] { viewportName }
                };

                // Create PNG save options and assign the rasterization options
                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Export the selected viewport to a PNG file
                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Export completed: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}