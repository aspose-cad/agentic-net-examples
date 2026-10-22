using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

class Program
{
    static void Main()
    {
        try
        {
            // Load the source CAD file
            string inputCadPath = "input.dwg";

            if (!File.Exists(inputCadPath))
            {
                Console.Error.WriteLine($"Input CAD file not found: {inputCadPath}");
                return;
            }

            using (Image cadImage = Image.Load(inputCadPath))
            {
                // Embed a blind watermark (text only)
                IWatermarkGuardService embedService = cadImage.WatermarkGuardService;
                embedService.TextToEmbed = "Confidential Owner";

                // Optionally check embedding status
                WatermarkingStatus embedStatus = embedService.EmbeddingStatus();
                if (embedStatus != WatermarkingStatus.SUCCESS)
                {
                    Console.Error.WriteLine($"Watermark embedding failed: {embedStatus}");
                    return;
                }

                // Prepare PDF save options
                PdfOptions pdfOptions = new PdfOptions();

                // Save the CAD image as PDF with the embedded watermark
                string outputPdfPath = "output.pdf";
                cadImage.Save(outputPdfPath, pdfOptions);
            }

            // Reload the generated PDF to validate the watermark
            if (!File.Exists("output.pdf"))
            {
                Console.Error.WriteLine("Failed to create output PDF.");
                return;
            }

            using (Image pdfImage = Image.Load("output.pdf"))
            {
                IWatermarkGuardService validateService = pdfImage.WatermarkGuardService;
                validateService.TextToValidate = "Confidential Owner";

                WatermarkingStatus validationStatus = validateService.ValidationStatus();

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
        }
    }
}