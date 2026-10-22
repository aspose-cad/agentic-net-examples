using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for input PLT and output PDF
            string inputPltPath = "input.plt";
            string outputPdfPath = "output.pdf";

            // Verify input file exists
            if (!File.Exists(inputPltPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPltPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPdfPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the PLT file as a CadImage
            using (Image cadImage = Image.Load(inputPltPath))
            {
                // Embed a blind text watermark
                cadImage.WatermarkGuardService.TextToEmbed = "Confidential";

                // Check embedding status
                WatermarkingStatus embedStatus = cadImage.WatermarkGuardService.EmbeddingStatus();
                Console.WriteLine("Embedding status: " + embedStatus);

                // Save the CAD drawing to PDF
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(outputPdfPath, pdfOptions);
            }

            // Reload the saved PDF to validate the watermark
            using (Image loadedImage = Image.Load(outputPdfPath))
            {
                // Set the expected watermark text for validation
                loadedImage.WatermarkGuardService.TextToValidate = "Confidential";

                // Get validation status
                WatermarkingStatus validationStatus = loadedImage.WatermarkGuardService.ValidationStatus();
                Console.WriteLine("Validation status: " + validationStatus);

                // Report the result
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
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}