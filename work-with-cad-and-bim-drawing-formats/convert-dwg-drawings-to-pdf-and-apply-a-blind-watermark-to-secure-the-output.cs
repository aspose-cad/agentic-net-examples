using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

namespace CadBlindWatermarkExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Paths for source DWG, intermediate watermarked DWG, and final PDF
                string sourceDwGPath = "input.dwg";
                string watermarkedDwGPath = "watermarked.dwg";
                string outputPdfPath = "output.pdf";

                // Verify source file exists
                if (!File.Exists(sourceDwGPath))
                {
                    Console.Error.WriteLine($"Source DWG file not found: {sourceDwGPath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPdfPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // -----------------------------------------------------------------
                // Step 1: Load the original DWG drawing
                // -----------------------------------------------------------------
                using (Image originalImage = Image.Load(sourceDwGPath))
                {
                    // Cast to CadImage for CAD‑specific operations
                    CadImage cadImage = (CadImage)originalImage;

                    // -----------------------------------------------------------------
                    // Step 2: Embed a blind text watermark before saving
                    // -----------------------------------------------------------------
                    cadImage.WatermarkGuardService.TextToEmbed = "Confidential – Do Not Distribute";

                    // Verify embedding status
                    WatermarkingStatus embedStatus = cadImage.WatermarkGuardService.EmbeddingStatus();
                    if (embedStatus != WatermarkingStatus.SUCCESS)
                    {
                        Console.Error.WriteLine($"Watermark embedding failed with status: {embedStatus}");
                        return;
                    }

                    // -----------------------------------------------------------------
                    // Step 3: Save the watermarked drawing back to DWG (preserving editability)
                    // -----------------------------------------------------------------
                    DwgOptions dwgSaveOptions = new DwgOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };
                    cadImage.Save(watermarkedDwGPath, dwgSaveOptions);
                }

                // Verify watermarked file was created
                if (!File.Exists(watermarkedDwGPath))
                {
                    Console.Error.WriteLine($"Failed to create watermarked DWG file: {watermarkedDwGPath}");
                    return;
                }

                // -----------------------------------------------------------------
                // Step 4: Reload the watermarked DWG and validate the blind watermark
                // -----------------------------------------------------------------
                using (Image watermarkedImage = Image.Load(watermarkedDwGPath))
                {
                    CadImage watermarkedCad = (CadImage)watermarkedImage;

                    // Set the expected text to validate
                    watermarkedCad.WatermarkGuardService.TextToValidate = "Confidential – Do Not Distribute";

                    // Perform validation
                    WatermarkingStatus validationStatus = watermarkedCad.WatermarkGuardService.ValidationStatus();
                    if (validationStatus != WatermarkingStatus.SUCCESS)
                    {
                        Console.Error.WriteLine($"Watermark validation failed with status: {validationStatus}");
                        return;
                    }

                    // -----------------------------------------------------------------
                    // Step 5: Convert the validated DWG to PDF
                    // -----------------------------------------------------------------
                    PdfOptions pdfSaveOptions = new PdfOptions();
                    watermarkedCad.Save(outputPdfPath, pdfSaveOptions);
                }

                Console.WriteLine("DWG successfully watermarked, validated, and converted to PDF.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}