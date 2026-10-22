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
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Apply blind watermark text
                IWatermarkGuardService watermarkService = cadImage.WatermarkGuardService;
                watermarkService.TextToEmbed = "Confidential";

                // Check embedding status
                WatermarkingStatus embedStatus = watermarkService.EmbeddingStatus();
                if (embedStatus != WatermarkingStatus.SUCCESS)
                {
                    Console.Error.WriteLine($"Watermark embedding failed: {embedStatus}");
                }

                // Save to PDF (PDF/A can be set via PdfOptions if needed)
                PdfOptions pdfOptions = new PdfOptions();
                // Example: pdfOptions.PdfAConformanceLevel = PdfAConformanceLevel.PdfA1b;

                cadImage.Save(outputPath, pdfOptions);
            }

            // Validate watermark after reloading the saved PDF as CAD (if supported)
            using (CadImage reloadedImage = (CadImage)Image.Load(outputPath))
            {
                IWatermarkGuardService validationService = reloadedImage.WatermarkGuardService;
                validationService.TextToValidate = "Confidential";

                WatermarkingStatus validationStatus = validationService.ValidationStatus();
                Console.WriteLine($"Watermark validation status: {validationStatus}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}