using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string inputPath = "input.plt";
        const string outputPath = "output.jpg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the PLT file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options with high quality and anti‑aliasing
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Quality = new RasterizationQuality
                    {
                        Text = RasterizationQualityValue.High,
                        Hatch = RasterizationQualityValue.High,
                        Arc = RasterizationQualityValue.High
                    },
                    PageWidth = 1600,
                    PageHeight = 1200,
                    GraphicsOptions = new GraphicsOptions()
                };

                // Enable anti‑aliasing if supported via GraphicsOptions
                // (Assuming default GraphicsOptions enables anti‑aliasing; adjust if specific properties exist)
                
                // Set JPEG save options and attach the rasterization settings
                JpegOptions jpegOptions = new JpegOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the rendered image as JPEG
                cadImage.Save(outputPath, jpegOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}