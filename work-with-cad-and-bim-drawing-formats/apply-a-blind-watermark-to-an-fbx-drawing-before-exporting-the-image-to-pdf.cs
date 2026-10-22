using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.WatermarkGuard;

namespace BlindWatermarkExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Paths for input FBX and output PDF
                string inputCadPath = "input.fbx";
                string outputPdfPath = "output.pdf";

                // Verify input file exists
                if (!File.Exists(inputCadPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputCadPath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPdfPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the FBX drawing as a CadImage
                using (Image cadImage = Image.Load(inputCadPath))
                {
                    // Set the blind watermark text to embed
                    cadImage.WatermarkGuardService.TextToEmbed = "Confidential";

                    // Verify that embedding succeeded
                    WatermarkingStatus embedStatus = cadImage.WatermarkGuardService.EmbeddingStatus();
                    if (embedStatus == WatermarkingStatus.SUCCESS)
                    {
                        Console.WriteLine("Blind watermark embedded successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to embed watermark. Status: {embedStatus}");
                    }

                    // Prepare PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Export the CAD drawing to PDF
                    cadImage.Save(outputPdfPath, pdfOptions);
                    Console.WriteLine($"CAD drawing exported to PDF: {outputPdfPath}");
                }

                // Reload the original CAD file to validate the embedded watermark
                using (Image validationImage = Image.Load(inputCadPath))
                {
                    // Set the expected watermark text for validation
                    validationImage.WatermarkGuardService.TextToValidate = "Confidential";

                    // Perform validation
                    WatermarkingStatus validationStatus = validationImage.WatermarkGuardService.ValidationStatus();

                    // Report validation result
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
                    else
                    {
                        Console.WriteLine($"Unexpected validation status: {validationStatus}");
                    }
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