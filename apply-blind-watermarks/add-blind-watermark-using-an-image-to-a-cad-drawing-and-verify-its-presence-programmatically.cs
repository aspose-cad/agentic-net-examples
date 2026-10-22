using Aspose.CAD.FileFormats.Cad;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

namespace CadWatermarkExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourcePath = "input.dwg";
            string watermarkPath = "watermark.png";
            string outputFolder = "output";
            string outputPath = Path.Combine(outputFolder, "output.dwg");

            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source CAD file not found: {sourcePath}");
                return;
            }

            if (!File.Exists(watermarkPath))
            {
                Console.Error.WriteLine($"Watermark image file not found: {watermarkPath}");
                return;
            }

            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            try
            {
                byte[] watermarkBytes = File.ReadAllBytes(watermarkPath);

                // Load source CAD and embed blind watermark
                using (Image image = Image.Load(sourcePath))
                {
                    CadImage cadImage = (CadImage)image;

                    // Embed watermark using WatermarkGuardService
                    IWatermarkGuardService guard = cadImage.WatermarkGuardService;
                    guard.ImageToEmbed = new MemoryStream(watermarkBytes);

                    // Save the CAD with embedded watermark
                    DwgOptions saveOptions = new DwgOptions();
                    cadImage.Save(outputPath, saveOptions);
                }

                // Verify watermark presence
                using (Image verifyImage = Image.Load(outputPath))
                {
                    CadImage verifyCad = (CadImage)verifyImage;
                    IWatermarkGuardService verifyGuard = verifyCad.WatermarkGuardService;
                    verifyGuard.ImageToValidate = new MemoryStream(watermarkBytes);

                    WatermarkingStatus status = verifyGuard.ValidationStatus();

                    if (status == WatermarkingStatus.SUCCESS)
                    {
                        Console.WriteLine("Watermark successfully added and verified.");
                    }
                    else
                    {
                        Console.WriteLine($"Watermark verification failed. Status: {status}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}