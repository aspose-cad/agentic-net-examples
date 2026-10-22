using System;
using System.IO;
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
            string inputPath = "input.dxf";
            string outputPath = "output.dxf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Embed a blind text watermark
                cadImage.WatermarkGuardService.TextToEmbed = "Confidential";

                // Check embedding status
                WatermarkingStatus embedStatus = cadImage.WatermarkGuardService.EmbeddingStatus();
                Console.WriteLine($"Embedding status: {embedStatus}");

                // Save preserving editable objects
                DxfOptions saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };
                cadImage.Save(outputPath, saveOptions);
            }

            // Reload the saved drawing to validate the watermark
            using (CadImage loadedImage = (CadImage)Image.Load(outputPath))
            {
                // Set expected watermark text for validation
                loadedImage.WatermarkGuardService.TextToValidate = "Confidential";

                // Perform validation
                WatermarkingStatus validationStatus = loadedImage.WatermarkGuardService.ValidationStatus();
                Console.WriteLine($"Validation status: {validationStatus}");

                // Report validation result
                if (validationStatus == WatermarkingStatus.SUCCESS)
                {
                    Console.WriteLine("Blind watermark validation succeeded.");
                }
                else if (validationStatus == WatermarkingStatus.FAILURE)
                {
                    Console.WriteLine("Blind watermark validation failed.");
                }
                else if (validationStatus == WatermarkingStatus.UNKNOWN)
                {
                    Console.WriteLine("Blind watermark validation status unknown.");
                }
                else if (validationStatus == WatermarkingStatus.PENDING)
                {
                    Console.WriteLine("Blind watermark validation pending.");
                }
                else
                {
                    Console.WriteLine("Unexpected validation status.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}