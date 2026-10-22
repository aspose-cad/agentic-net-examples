using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.WatermarkGuard;

class Program
{
    static void Main(string[] args)
    {
        // Path to the CAD file to be checked
        string inputPath = "input.dwg";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Expected blind watermark text
        string expectedWatermark = "Confidential";

        // Temporary file for round‑trip validation
        string tempPath = Path.Combine(Path.GetTempPath(), "temp_watermarked.dwg");
        string tempDir = Path.GetDirectoryName(tempPath);
        if (!string.IsNullOrEmpty(tempDir) && !Directory.Exists(tempDir))
        {
            Directory.CreateDirectory(tempDir);
        }

        // Load the original CAD file
        using (Image loadedImage = Image.Load(inputPath))
        {
            CadImage cadImage = loadedImage as CadImage;
            if (cadImage == null)
            {
                Console.WriteLine("Failed to load CAD image.");
                return;
            }

            // Obtain the watermark guard service and set the text to validate
            IWatermarkGuardService watermarkService = cadImage.WatermarkGuardService;
            watermarkService.TextToValidate = expectedWatermark;

            // Save to a temporary file (preserving editable objects)
            DwgOptions saveOptions = new DwgOptions();
            cadImage.Save(tempPath, saveOptions);
        }

        // Reload the saved file to perform validation
        using (Image reloadedImage = Image.Load(tempPath))
        {
            CadImage reloadedCad = reloadedImage as CadImage;
            if (reloadedCad == null)
            {
                Console.WriteLine("Failed to reload CAD image.");
                return;
            }

            // Validate the blind watermark
            IWatermarkGuardService validationService = reloadedCad.WatermarkGuardService;
            validationService.TextToValidate = expectedWatermark;
            WatermarkingStatus status = validationService.ValidationStatus();

            if (status == WatermarkingStatus.SUCCESS)
            {
                Console.WriteLine("Blind watermark validation SUCCESS.");
            }
            else
            {
                Console.WriteLine($"Blind watermark validation FAILED. Status: {status}");
            }
        }

        // Clean up the temporary file
        try
        {
            File.Delete(tempPath);
        }
        catch
        {
            // Ignored
        }
    }
}