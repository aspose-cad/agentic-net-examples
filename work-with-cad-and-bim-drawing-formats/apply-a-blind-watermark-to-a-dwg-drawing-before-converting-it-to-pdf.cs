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
        // Paths for input DWG and output PDF
        string dwgPath = "input.dwg";
        string pdfPath = "output.pdf";

        // Verify input file exists
        if (!File.Exists(dwgPath))
        {
            Console.Error.WriteLine($"Input file not found: {dwgPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(pdfPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the DWG drawing
            using (Image loadedImage = Image.Load(dwgPath))
            {
                CadImage cadImage = (CadImage)loadedImage;

                // Access the watermark guard service
                IWatermarkGuardService watermarkService = cadImage.WatermarkGuardService;

                // Set the blind watermark text to embed
                watermarkService.TextToEmbed = "Confidential";

                // Verify that embedding succeeded
                WatermarkingStatus embedStatus = watermarkService.EmbeddingStatus();
                if (embedStatus != WatermarkingStatus.SUCCESS)
                {
                    Console.WriteLine($"Embedding failed with status: {embedStatus}");
                    return;
                }

                // Save the drawing as PDF
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(pdfPath, pdfOptions);
            }

            // Re‑load the DWG to validate the embedded watermark
            using (Image validateImage = Image.Load(dwgPath))
            {
                CadImage cadValidate = (CadImage)validateImage;
                IWatermarkGuardService validateService = cadValidate.WatermarkGuardService;

                // Set the expected watermark text for validation
                validateService.TextToValidate = "Confidential";

                // Perform validation
                WatermarkingStatus validationStatus = validateService.ValidationStatus();

                if (validationStatus == WatermarkingStatus.SUCCESS)
                {
                    Console.WriteLine("Blind watermark validation succeeded.");
                }
                else
                {
                    Console.WriteLine($"Blind watermark validation failed with status: {validationStatus}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}