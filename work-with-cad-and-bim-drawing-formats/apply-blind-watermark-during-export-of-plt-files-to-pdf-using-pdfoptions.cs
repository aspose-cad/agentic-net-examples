using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

namespace BlindWatermarkExample
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
                // Load the PLT file
                using (Image cadImage = Image.Load(inputPltPath))
                {
                    // Set blind watermark text to embed
                    cadImage.WatermarkGuardService.TextToEmbed = "Confidential";

                    // Prepare PDF save options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the CAD image as PDF with the embedded watermark
                    cadImage.Save(outputPdfPath, pdfOptions);
                }

                // Reload the saved PDF to validate the watermark
                using (Image pdfImage = Image.Load(outputPdfPath))
                {
                    // Set the expected watermark text for validation
                    pdfImage.WatermarkGuardService.TextToValidate = "Confidential";

                    // Perform validation
                    WatermarkingStatus validationStatus = pdfImage.WatermarkGuardService.ValidationStatus();

                    // Report the validation result
                    if (validationStatus == WatermarkingStatus.SUCCESS)
                    {
                        Console.WriteLine("Watermark validation succeeded.");
                    }
                    else if (validationStatus == WatermarkingStatus.FAILURE)
                    {
                        Console.WriteLine("Watermark validation failed.");
                    }
                    else if (validationStatus == WatermarkingStatus.UNKNOWN)
                    {
                        Console.WriteLine("Watermark validation status is unknown.");
                    }
                    else if (validationStatus == WatermarkingStatus.PENDING)
                    {
                        Console.WriteLine("Watermark validation is pending.");
                    }
                    else
                    {
                        Console.WriteLine($"Watermark validation returned unexpected status: {validationStatus}");
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