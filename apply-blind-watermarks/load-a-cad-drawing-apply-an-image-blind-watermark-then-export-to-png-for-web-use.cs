using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.WatermarkGuard;

class Program
{
    static void Main()
    {
        try
        {
            // Paths to the source CAD file and the watermark image
            string cadFilePath = "sample.dwg";
            string watermarkFilePath = "watermark.png";

            // Prepare output directory
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
            Directory.CreateDirectory(outputDir);
            string pngOutputPath = Path.Combine(outputDir, "output.png");

            // Verify required files exist
            if (!File.Exists(cadFilePath))
            {
                Console.WriteLine($"CAD source file not found: {cadFilePath}");
                return;
            }
            if (!File.Exists(watermarkFilePath))
            {
                Console.WriteLine($"Watermark image file not found: {watermarkFilePath}");
                return;
            }

            // Load CAD drawing, embed blind image watermark, and save as PNG
            using (FileStream watermarkStream = File.OpenRead(watermarkFilePath))
            {
                // Load as CadImage to access CAD‑specific members
                CadImage cadImage = (CadImage)Image.Load(cadFilePath);

                // Embed the watermark image
                cadImage.WatermarkGuardService.ImageToEmbed = watermarkStream;

                // Configure PNG save options with rasterization settings
                PngOptions pngOptions = new PngOptions();
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800,
                    PageHeight = 600,
                    // Layouts expects an array of layout names (strings)
                    Layouts = cadImage.Layouts.Keys.ToArray()
                };
                pngOptions.VectorRasterizationOptions = rasterOptions;

                // Save the watermarked image
                cadImage.Save(pngOutputPath, pngOptions);
            }

            // Validate that the watermark is present in the saved PNG
            using (FileStream watermarkValidateStream = File.OpenRead(watermarkFilePath))
            {
                Image savedImage = Image.Load(pngOutputPath);
                savedImage.WatermarkGuardService.ImageToValidate = watermarkValidateStream;
                WatermarkingStatus validationStatus = savedImage.WatermarkGuardService.ValidationStatus();
                Console.WriteLine($"Watermark validation status: {validationStatus}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}