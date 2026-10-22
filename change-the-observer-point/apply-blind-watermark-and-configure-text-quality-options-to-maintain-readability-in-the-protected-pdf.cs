using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

class Program
{
    static void Main()
    {
        // Paths for input CAD file and output PDF
        string inputCadPath = "sample.dwg";
        string outputPdfPath = "output.pdf";

        // Verify input file exists
        if (!File.Exists(inputCadPath))
        {
            Console.Error.WriteLine($"Input CAD file not found: {inputCadPath}");
            return;
        }

        try
        {
            // Load the CAD file
            using (Image cadImage = Image.Load(inputCadPath))
            {
                // Set blind watermark text to embed
                cadImage.WatermarkGuardService.TextToEmbed = "Confidential";

                // Check embedding status before saving
                WatermarkingStatus embedStatus = cadImage.WatermarkGuardService.EmbeddingStatus();
                Console.WriteLine($"Embedding status before save: {embedStatus}");

                // Configure PDF save options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1200,
                        PageHeight = 800
                    }
                };

                // Save the CAD image as PDF (watermark will be embedded)
                cadImage.Save(outputPdfPath, pdfOptions);
            }

            // Reload the saved PDF to validate the blind watermark
            using (FileStream pdfStream = new FileStream(outputPdfPath, FileMode.Open, FileAccess.Read))
            using (Image pdfImage = Image.Load(pdfStream))
            {
                // Set expected watermark text for validation
                pdfImage.WatermarkGuardService.TextToValidate = "Confidential";

                // Perform validation
                WatermarkingStatus validationStatus = pdfImage.WatermarkGuardService.ValidationStatus();
                Console.WriteLine($"Validation status: {validationStatus}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}