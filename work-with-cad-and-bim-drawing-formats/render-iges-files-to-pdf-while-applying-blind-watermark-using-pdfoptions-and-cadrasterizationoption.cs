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
            string inputPath = "sample.iges";
            string outputPdfPath = "output.pdf";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDir = Path.GetDirectoryName(outputPdfPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the IGES file
            using (Image cadImage = Image.Load(inputPath))
            {
                CadImage cad = (CadImage)cadImage;

                // Embed a blind text watermark
                cad.WatermarkGuardService.TextToEmbed = "Confidential";

                // Verify embedding status
                WatermarkingStatus embedStatus = cad.WatermarkGuardService.EmbeddingStatus();
                Console.WriteLine($"Embedding status: {embedStatus}");

                // Set up PDF options
                PdfOptions pdfOptions = new PdfOptions();

                // NOTE: VectorRasterizationOptions is abstract in this SDK version;
                // a concrete implementation is not available, so default options are used.

                // Save the CAD drawing as PDF
                cad.Save(outputPdfPath, pdfOptions);
            }

            // Attempt to reload the saved PDF for watermark validation
            try
            {
                using (Image loadedImage = Image.Load(outputPdfPath))
                {
                    CadImage loadedCad = (CadImage)loadedImage;

                    // Set the expected watermark text for validation
                    loadedCad.WatermarkGuardService.TextToValidate = "Confidential";

                    // Check validation status
                    WatermarkingStatus validationStatus = loadedCad.WatermarkGuardService.ValidationStatus();
                    Console.WriteLine($"Validation status: {validationStatus}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to load PDF for validation: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}