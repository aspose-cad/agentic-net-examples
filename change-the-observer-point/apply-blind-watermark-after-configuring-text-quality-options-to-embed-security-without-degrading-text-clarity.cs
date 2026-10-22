using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.WatermarkGuard;

namespace CadBlindWatermarkExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input CAD file path
                string inputCadPath = "input.dwg";
                // Output PDF file path
                string outputPdfPath = "output.pdf";

                // Verify input file exists
                if (!File.Exists(inputCadPath))
                {
                    Console.Error.WriteLine($"Input CAD file not found: {inputCadPath}");
                    return;
                }

                // Load the CAD drawing
                using (Image cadImage = Image.Load(inputCadPath))
                {
                    // Cast to CadImage to access CAD‑specific members if needed
                    CadImage cad = (CadImage)cadImage;

                    // Configure blind watermark (text only, no visible overlay)
                    cadImage.WatermarkGuardService.TextToEmbed = "Confidential";

                    // Configure PDF options (example: preserve text quality)
                    PdfOptions pdfOptions = new PdfOptions();
                    // If PdfOptions exposes a text quality setting, it can be set here.
                    // pdfOptions.TextQuality = 100; // placeholder for actual property

                    // Save the drawing to PDF (rasterized vector export)
                    cadImage.Save(outputPdfPath, pdfOptions);
                }

                // Verify output file exists before attempting to load it
                if (!File.Exists(outputPdfPath))
                {
                    Console.Error.WriteLine($"Output PDF file not created: {outputPdfPath}");
                    return;
                }

                // Reload the saved PDF to validate the embedded watermark
                using (Image loadedImage = Image.Load(outputPdfPath))
                {
                    // Set the expected text to validate
                    loadedImage.WatermarkGuardService.TextToValidate = "Confidential";

                    // Perform validation
                    WatermarkingStatus validationStatus = loadedImage.WatermarkGuardService.ValidationStatus();

                    // Report validation result
                    switch (validationStatus)
                    {
                        case WatermarkingStatus.SUCCESS:
                            Console.WriteLine("Watermark validation succeeded.");
                            break;
                        case WatermarkingStatus.FAILURE:
                            Console.WriteLine("Watermark validation failed.");
                            break;
                        case WatermarkingStatus.PENDING:
                            Console.WriteLine("Watermark validation pending.");
                            break;
                        default:
                            Console.WriteLine("Watermark validation returned unknown status.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}