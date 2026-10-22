using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.pdf";

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image cadImage = Image.Load(inputPath))
            {
                // Apply blind watermark
                IWatermarkGuardService watermarkService = cadImage.WatermarkGuardService;
                watermarkService.TextToEmbed = "Confidential";

                // Verify embedding status
                WatermarkingStatus embedStatus = watermarkService.EmbeddingStatus();
                if (embedStatus != WatermarkingStatus.SUCCESS)
                {
                    Console.Error.WriteLine($"Watermark embedding failed with status: {embedStatus}");
                    return;
                }

                // Configure rasterization options with margins
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.UnitType = UnitType.Centimenter;
                rasterOptions.LineScale = 1.0f;

                Margins margins = new Margins();
                margins.Left = 10;
                margins.Right = 10;
                margins.Top = 10;
                margins.Bottom = 10;
                rasterOptions.Margins = margins;

                // Set up PDF save options
                PdfOptions pdfOptions = new PdfOptions();
                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save to PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            // Reload the saved PDF to validate watermark (if supported)
            using (Image pdfImage = Image.Load(outputPath))
            {
                IWatermarkGuardService validationService = pdfImage.WatermarkGuardService;
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