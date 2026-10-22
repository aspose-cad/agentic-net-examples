using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

namespace CadBlindWatermarkSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define input and output paths
                string inputPath = "input.dwg";
                string outputDirectory = "output";
                string outputPath = Path.Combine(outputDirectory, "watermarked.dxf");

                // Verify input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input CAD file not found: {inputPath}");
                    return;
                }

                // Ensure output directory exists
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Load the source CAD image
                using (Image sourceImage = Image.Load(inputPath))
                {
                    CadImage cadImage = sourceImage as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Failed to load CAD image.");
                        return;
                    }

                    // Embed blind text watermark using the WatermarkGuardService
                    IWatermarkGuardService embedService = cadImage.WatermarkGuardService;
                    embedService.TextToEmbed = "Confidential";

                    // Save the watermarked CAD file using DXF format (preserves editable objects)
                    DxfOptions dxfOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };
                    cadImage.Save(outputPath, dxfOptions);

                    // Retrieve embedding status
                    WatermarkingStatus embedStatus = embedService.EmbeddingStatus();
                    Console.WriteLine($"Embedding status: {embedStatus}");
                }

                // Reload the saved CAD file for validation
                using (Image savedImage = Image.Load(outputPath))
                {
                    CadImage loadedCadImage = savedImage as CadImage;
                    if (loadedCadImage == null)
                    {
                        Console.Error.WriteLine("Failed to reload saved CAD image.");
                        return;
                    }

                    // Validate blind text watermark using the WatermarkGuardService
                    IWatermarkGuardService validateService = loadedCadImage.WatermarkGuardService;
                    validateService.TextToValidate = "Confidential";

                    WatermarkingStatus validationStatus = validateService.ValidationStatus();
                    Console.WriteLine($"Validation status: {validationStatus}");

                    if (validationStatus == WatermarkingStatus.SUCCESS)
                    {
                        Console.WriteLine("Blind watermark successfully validated.");
                    }
                    else
                    {
                        Console.WriteLine("Blind watermark validation failed or is inconclusive.");
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