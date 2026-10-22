using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class RenderPltToJpeg
{
    static void Main()
    {
        try
        {
            string sourcePath = "input.plt";
            string outputPath = "output.jpg";

            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            var cadImage = Image.Load(sourcePath) as CadImage;
            if (cadImage == null)
            {
                Console.Error.WriteLine("Failed to load the PLT file as a CAD image.");
                return;
            }

            var rasterOptions = new CadRasterizationOptions
            {
                BackgroundColor = Aspose.CAD.Color.White,
                PageWidth = (float)cadImage.Width,
                PageHeight = (float)cadImage.Height,
                AutomaticLayoutsScaling = true,
                GraphicsOptions = new GraphicsOptions
                {
                    SmoothingMode = SmoothingMode.AntiAlias,
                    TextRenderingHint = TextRenderingHint.AntiAliasGridFit
                },
                Quality = new RasterizationQuality
                {
                    Text = RasterizationQualityValue.High,
                    Hatch = RasterizationQualityValue.High,
                    Arc = RasterizationQualityValue.High
                }
            };

            var jpegOptions = new JpegOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            cadImage.Save(outputPath, jpegOptions);

            Console.WriteLine("PLT file has been rendered to JPEG with anti-aliasing and white background.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}