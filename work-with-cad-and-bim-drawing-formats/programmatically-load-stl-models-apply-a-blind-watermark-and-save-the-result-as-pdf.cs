using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

class Program
{
    static void Main()
    {
        string inputStlPath = "model.stl";
        string outputPdfPath = "watermarked.pdf";

        // Verify input STL exists
        if (!File.Exists(inputStlPath))
        {
            Console.Error.WriteLine($"Input STL file not found: {inputStlPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPdfPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the STL model
            Image cadImage = Image.Load(inputStlPath);

            // Apply blind watermark
            IWatermarkGuardService watermarkService = cadImage.WatermarkGuardService;
            watermarkService.TextToEmbed = "Confidential";

            // Check embedding status
            WatermarkingStatus embedStatus = watermarkService.EmbeddingStatus();
            Console.WriteLine($"Embedding status: {embedStatus}");

            // Save as PDF
            PdfOptions pdfOptions = new PdfOptions();
            cadImage.Save(outputPdfPath, pdfOptions);
            cadImage.Dispose();

            // Verify output PDF was created
            if (!File.Exists(outputPdfPath))
            {
                Console.Error.WriteLine($"Failed to create output PDF: {outputPdfPath}");
                return;
            }

            // Load the saved PDF to validate the blind watermark
            Image loadedPdf = Image.Load(outputPdfPath);
            IWatermarkGuardService validateService = loadedPdf.WatermarkGuardService;
            validateService.TextToValidate = "Confidential";

            // Perform validation
            WatermarkingStatus validationStatus = validateService.ValidationStatus();
            Console.WriteLine($"Validation status: {validationStatus}");

            loadedPdf.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}