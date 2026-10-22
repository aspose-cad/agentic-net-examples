using Aspose.CAD.FileFormats.Cad;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

class Program
{
    static void Main()
    {
        // Paths to source DWG and target PNG
        string sourceDwGPath = "input.dwg";
        string targetPngPath = "output.png";

        // Verify source file exists
        if (!File.Exists(sourceDwGPath))
        {
            Console.WriteLine($"Source file not found: {sourceDwGPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(targetPngPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the DWG drawing
            using (Image loadedImage = Image.Load(sourceDwGPath))
            {
                CadImage cadImage = loadedImage as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to load the DWG file as a CadImage.");
                    return;
                }

                // Apply a blind text watermark
                IWatermarkGuardService watermarkService = cadImage.WatermarkGuardService;
                watermarkService.TextToEmbed = "Confidential";

                // Export the watermarked drawing to PNG
                PngOptions pngOptions = new PngOptions();
                cadImage.Save(targetPngPath, pngOptions);

                // Verify embedding status
                WatermarkingStatus embedStatus = watermarkService.EmbeddingStatus();
                if (embedStatus == WatermarkingStatus.SUCCESS)
                {
                    Console.WriteLine("Blind watermark embedded successfully.");
                }
                else
                {
                    Console.WriteLine($"Watermark embedding failed with status: {embedStatus}");
                }
            }

            // Re‑load the DWG to validate the watermark
            using (Image validationImage = Image.Load(sourceDwGPath))
            {
                CadImage cadValidate = validationImage as CadImage;
                if (cadValidate == null)
                {
                    Console.WriteLine("Failed to reload the DWG file for validation.");
                    return;
                }

                IWatermarkGuardService validationService = cadValidate.WatermarkGuardService;
                validationService.TextToValidate = "Confidential";

                // Check validation status
                WatermarkingStatus validationStatus = validationService.ValidationStatus();
                if (validationStatus == WatermarkingStatus.SUCCESS)
                {
                    Console.WriteLine("Blind watermark validation succeeded.");
                }
                else
                {
                    Console.WriteLine($"Blind watermark validation result: {validationStatus}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}