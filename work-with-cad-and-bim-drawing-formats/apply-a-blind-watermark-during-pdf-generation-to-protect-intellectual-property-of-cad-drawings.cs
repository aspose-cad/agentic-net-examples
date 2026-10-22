using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

namespace CadBlindWatermarkPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Paths for input CAD file and output PDF
                string inputCadPath = "sample.dwg";
                string outputPdfPath = "sample_watermarked.pdf";

                // Verify input file exists
                if (!File.Exists(inputCadPath))
                {
                    Console.Error.WriteLine($"Input CAD file not found: {inputCadPath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPdfPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the CAD drawing
                using (CadImage cadImage = (CadImage)Image.Load(inputCadPath))
                {
                    // ----- Embed blind watermark (text) -----
                    cadImage.WatermarkGuardService.TextToEmbed = "Confidential – Do Not Distribute";

                    // Verify that embedding succeeded
                    WatermarkingStatus embedStatus = cadImage.WatermarkGuardService.EmbeddingStatus();
                    Console.WriteLine($"Embedding status: {embedStatus}");

                    // Save the drawing to PDF; the embedded watermark travels with the file
                    PdfOptions pdfOptions = new PdfOptions();
                    cadImage.Save(outputPdfPath, pdfOptions);
                }

                // ----- Validate the blind watermark after saving -----
                // Reload the generated PDF (Aspose.CAD can load PDF as a CAD image)
                using (Image loadedImage = Image.Load(outputPdfPath))
                {
                    CadImage loadedCad = loadedImage as CadImage;
                    if (loadedCad != null)
                    {
                        // Set the expected watermark text for validation
                        loadedCad.WatermarkGuardService.TextToValidate = "Confidential – Do Not Distribute";

                        // Perform validation
                        WatermarkingStatus validationStatus = loadedCad.WatermarkGuardService.ValidationStatus();
                        Console.WriteLine($"Validation status: {validationStatus}");

                        // Report result based on status
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
                    else
                    {
                        Console.WriteLine("The loaded file is not a CAD image; cannot perform watermark validation.");
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