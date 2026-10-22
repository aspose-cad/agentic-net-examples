using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.WatermarkGuard;

namespace PltToPdfWithBlindWatermark
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input PLT file path
            string inputPltPath = "input.plt";
            // Output PDF file path
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

            try
            {
                // Load the PLT file as a CadImage
                using (Image cadImage = Image.Load(inputPltPath))
                {
                    // ----- Blind watermark embedding -----
                    // Set the text to embed as a blind watermark
                    cadImage.WatermarkGuardService.TextToEmbed = "Confidential";

                    // Save the CAD image to PDF using PdfOptions
                    PdfOptions pdfOptions = new PdfOptions();

                    // VectorRasterizationOptions is abstract in this SDK version; use a concrete implementation
                    var vectorOptions = new CadRasterizationOptions();
                    pdfOptions.VectorRasterizationOptions = vectorOptions;

                    cadImage.Save(outputPdfPath, pdfOptions);

                    // Check embedding status
                    WatermarkingStatus embedStatus = cadImage.WatermarkGuardService.EmbeddingStatus();
                    Console.WriteLine($"Embedding status: {embedStatus}");
                }

                // ----- Validation of the blind watermark -----
                // Reload the saved PDF to validate the watermark
                using (Image loadedImage = Image.Load(outputPdfPath))
                {
                    // Set the expected text to validate
                    loadedImage.WatermarkGuardService.TextToValidate = "Confidential";

                    // Perform validation
                    WatermarkingStatus validationStatus = loadedImage.WatermarkGuardService.ValidationStatus();
                    Console.WriteLine($"Validation status: {validationStatus}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}