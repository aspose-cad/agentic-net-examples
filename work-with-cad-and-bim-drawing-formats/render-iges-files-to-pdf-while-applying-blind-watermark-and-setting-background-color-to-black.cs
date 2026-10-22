using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

class Program
{
    static void Main()
    {
        string inputPath = "input.igs";
        string outputPath = "output.pdf";

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
            // Load the IGES file
            var image = Image.Load(inputPath);

            // Embed a blind text watermark before saving
            image.WatermarkGuardService.TextToEmbed = "Confidential";

            // Check embedding status
            WatermarkingStatus embedStatus = image.WatermarkGuardService.EmbeddingStatus();
            Console.WriteLine($"Embedding status: {embedStatus}");

            // Prepare PDF options (background color not supported directly)
            var pdfOptions = new PdfOptions();

            // Save the rendered PDF
            image.Save(outputPath, pdfOptions);

            // Reload the saved PDF to validate the blind watermark
            var result = Image.Load(outputPath);
            result.WatermarkGuardService.TextToValidate = "Confidential";

            WatermarkingStatus validationStatus = result.WatermarkGuardService.ValidationStatus();
            Console.WriteLine($"Validation status: {validationStatus}");

            if (validationStatus == WatermarkingStatus.SUCCESS)
            {
                Console.WriteLine("Blind watermark successfully validated.");
            }
            else
            {
                Console.WriteLine($"Blind watermark validation failed: {validationStatus}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}