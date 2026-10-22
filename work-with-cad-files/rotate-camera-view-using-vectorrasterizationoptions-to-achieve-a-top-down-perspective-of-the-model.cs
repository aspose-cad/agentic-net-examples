using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "model.dwg";
        string outputPath = "output/topdown.png";

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists
            string outDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outDir))
                Directory.CreateDirectory(outDir);

            // Load the CAD image
            Image cadImage = Image.Load(inputPath);

            // Configure rasterization options for a top‑down view
            // CadRasterizationOptions is a concrete implementation of VectorRasterizationOptions
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                ContentAsBitmap = true
                // The Aspose.CAD API does not provide a direct property to rotate the camera.
                // Rotation would normally be applied via a transformation matrix in GraphicsOptions,
                // which requires System.Drawing.Drawing2D.Matrix – a prohibited namespace.
                // Therefore, the sample uses the default view without explicit rotation.
            };

            // Assign default graphics options (no transformation)
            rasterOptions.GraphicsOptions = new GraphicsOptions();

            // Set up PNG options with the configured rasterization options
            PngOptions pngOptions = new PngOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save the resulting image
            cadImage.Save(outputPath, pngOptions);
            Console.WriteLine($"Image saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}