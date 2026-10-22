using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;
using System.IO;

namespace CadWatermarkExample
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Input FBX file path
                    string inputPath = "input.fbx";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Output PDF file path
                    string outputPath = "output.pdf";
                    // Blind watermark text to embed
                    string watermarkText = "Confidential";

                    // Load the FBX drawing as a CAD image
                    using (Image cadImage = Image.Load(inputPath))
                    {
                        // Embed blind watermark text
                        cadImage.WatermarkGuardService.TextToEmbed = watermarkText;

                        // Verify embedding status
                        WatermarkingStatus embedStatus = cadImage.WatermarkGuardService.EmbeddingStatus();
                        Console.WriteLine($"Embedding status: {embedStatus}");

                        // Require successful embedding before proceeding
                        if (embedStatus != WatermarkingStatus.SUCCESS)
                        {
                            Console.WriteLine("Failed to embed watermark. Aborting.");
                            return;
                        }

                        // Prepare PDF export options
                        PdfOptions pdfOptions = new PdfOptions();

                        // Save the CAD drawing to PDF with the embedded watermark
                        cadImage.Save(outputPath, pdfOptions);
                    }

                    // Reload the saved PDF to validate the blind watermark
                    using (Image loadedPdf = Image.Load(outputPath))
                    {
                        // Set the expected watermark text for validation
                        loadedPdf.WatermarkGuardService.TextToValidate = watermarkText;

                        // Perform validation
                        WatermarkingStatus validationStatus = loadedPdf.WatermarkGuardService.ValidationStatus();
                        Console.WriteLine($"Validation status: {validationStatus}");

                        // Report result based on validation outcome
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
                            Console.WriteLine("Blind watermark validation status is unknown.");
                        }
                        else if (validationStatus == WatermarkingStatus.PENDING)
                        {
                            Console.WriteLine("Blind watermark validation is pending.");
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