using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;
using Aspose.CAD.FileFormats.Cad;

namespace CadWatermarkExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input CAD file path (first argument or default)
            string inputPath = args.Length > 0 ? args[0] : "sample.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare output directory
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Define output file path (same format as input)
            string outputPath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(inputPath) + "_watermarked.dxf");

            try
            {
                // Load the CAD image
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Embed blind text watermark
                    cadImage.WatermarkGuardService.TextToEmbed = "Confidential";

                    // Save with native CAD options preserving editability
                    DxfOptions saveOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };
                    cadImage.Save(outputPath, saveOptions);
                }

                // Reload the saved file for validation
                using (Image loadedImage = Image.Load(outputPath))
                {
                    // Set expected watermark text for validation
                    loadedImage.WatermarkGuardService.TextToValidate = "Confidential";

                    // Perform validation
                    WatermarkingStatus status = loadedImage.WatermarkGuardService.ValidationStatus();

                    // Report result
                    if (status == WatermarkingStatus.SUCCESS)
                    {
                        Console.WriteLine("Watermark validation succeeded.");
                    }
                    else if (status == WatermarkingStatus.FAILURE)
                    {
                        Console.WriteLine("Watermark validation failed.");
                    }
                    else if (status == WatermarkingStatus.PENDING)
                    {
                        Console.WriteLine("Watermark validation pending.");
                    }
                    else
                    {
                        Console.WriteLine($"Watermark validation returned unknown status: {status}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}