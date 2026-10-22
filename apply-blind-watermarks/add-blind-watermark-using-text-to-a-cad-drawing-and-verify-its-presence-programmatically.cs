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
                // Paths
                string inputPath = "input.dwg";
                string outputDirectory = "Output";
                string outputPath = Path.Combine(outputDirectory, "output.dwg");
                string watermarkText = "Confidential";

                // Verify input file exists
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file '{inputPath}' not found. Exiting.");
                    return;
                }

                // Ensure output directory exists
                Directory.CreateDirectory(outputDirectory);

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // Embed blind watermark text
                    IWatermarkGuardService watermarkService = cadImage.WatermarkGuardService;
                    watermarkService.TextToEmbed = watermarkText;

                    // Save the drawing preserving editable objects
                    DwgOptions saveOptions = new DwgOptions();
                    cadImage.Save(outputPath, saveOptions);
                }

                // Reload the saved drawing and validate the watermark
                using (Image savedImage = Image.Load(outputPath))
                {
                    CadImage savedCadImage = savedImage as CadImage;
                    if (savedCadImage == null)
                    {
                        Console.WriteLine("Failed to cast saved image to CadImage.");
                        return;
                    }

                    IWatermarkGuardService validationService = savedCadImage.WatermarkGuardService;
                    validationService.TextToValidate = watermarkText;

                    WatermarkingStatus validationStatus = validationService.ValidationStatus();

                    if (validationStatus == WatermarkingStatus.SUCCESS)
                    {
                        Console.WriteLine("Watermark validation succeeded.");
                    }
                    else
                    {
                        Console.WriteLine($"Watermark validation failed: {validationStatus}");
                    }
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}