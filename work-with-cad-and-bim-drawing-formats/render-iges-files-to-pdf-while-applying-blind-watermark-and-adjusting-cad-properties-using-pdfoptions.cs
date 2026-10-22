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
            // Paths for input IGES file and output PDF
            string inputPath = "sample.iges";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputPdfPath = "output.pdf";

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPdfPath);
            if (!string.IsNullOrEmpty(outputDir))
                Directory.CreateDirectory(outputDir);

            // Load the IGES file
            using (Image cadImage = Image.Load(inputPath))
            {
                Image cad = cadImage ;
                if (cad == null)
                {
                    Console.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Apply a blind text watermark before saving
                cad.WatermarkGuardService.TextToEmbed = "Confidential";

                // Prepare PDF save options (VectorRasterizationOptions not required)
                PdfOptions pdfOptions = new PdfOptions();

                // Save the CAD image as PDF with the embedded watermark
                cad.Save(outputPdfPath, pdfOptions);
            }

            // Reload the saved PDF to validate the watermark
            using (Image loadedPdf = Image.Load(outputPdfPath))
            {
                // Specify the expected watermark text for validation
                loadedPdf.WatermarkGuardService.TextToValidate = "Confidential";

                // Retrieve the validation status
                WatermarkingStatus status = loadedPdf.WatermarkGuardService.ValidationStatus();

                // Report the validation result
                if (status == WatermarkingStatus.SUCCESS)
                {
                    Console.WriteLine("Watermark validation succeeded.");
                }
                else if (status == WatermarkingStatus.FAILURE)
                {
                    Console.WriteLine("Watermark validation failed.");
                }
                else if (status == WatermarkingStatus.UNKNOWN)
                {
                    Console.WriteLine("Watermark validation status unknown.");
                }
                else if (status == WatermarkingStatus.PENDING)
                {
                    Console.WriteLine("Watermark validation pending.");
                }
                else
                {
                    Console.WriteLine($"Watermark validation returned unexpected status: {status}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}