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
            string inputPath = "input.dwg";
            string pdfPath = "output.pdf";
            string watermarkText = "Confidential";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image cadImage = Image.Load(inputPath))
            {
                // Embed blind watermark
                IWatermarkGuardService embedService = cadImage.WatermarkGuardService;
                embedService.TextToEmbed = watermarkText;

                // Save CAD as PDF
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(pdfPath, pdfOptions);

                // Report embedding status
                Console.WriteLine($"Embedding status: {embedService.EmbeddingStatus()}");

                // Validate watermark after reloading the saved PDF
                using (Image savedImage = Image.Load(pdfPath))
                {
                    IWatermarkGuardService validateService = savedImage.WatermarkGuardService;
                    validateService.TextToValidate = watermarkText;
                    Console.WriteLine($"Validation status: {validateService.ValidationStatus()}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}