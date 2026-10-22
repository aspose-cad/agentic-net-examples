using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        string inputPath = "input.dwg";
        string outputPath = "output.png";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load CAD image (non-generic method)
            Image img = Image.Load(inputPath);
            CadImage cadImage = img as CadImage;
            if (cadImage == null)
            {
                Console.WriteLine("Failed to load CAD image.");
                return;
            }

            // Original drawing dimensions (in drawing units)
            double originalWidth = cadImage.Size.Width;
            double originalHeight = cadImage.Size.Height;
            double originalAspect = originalWidth / originalHeight;

            // Rasterization options – let Aspose.CAD determine size automatically
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions();

            // PNG output options
            PngOptions pngOptions = new PngOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Ensure the output file does not already exist
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            // Save the CAD drawing as PNG
            cadImage.Save(outputPath, pngOptions);

            Console.WriteLine($"Original aspect ratio: {originalAspect:F4}");
            Console.WriteLine($"Image saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}
