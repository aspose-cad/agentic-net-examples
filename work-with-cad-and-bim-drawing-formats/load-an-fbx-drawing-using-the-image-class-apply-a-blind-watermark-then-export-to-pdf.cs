using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

namespace CadWatermarkExample
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Paths for input FBX and output PDF
                    string inputPath = "input.fbx";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        string outputPath = "output.pdf";

                    // Text to embed as a blind watermark
                    string watermarkText = "Confidential";

                    // Load the FBX drawing
                    using (Image cadImage = Image.Load(inputPath))
                    {
                        // Set the text to embed
                        cadImage.WatermarkGuardService.TextToEmbed = watermarkText;

                        // Verify embedding status
                        WatermarkingStatus embedStatus = cadImage.WatermarkGuardService.EmbeddingStatus();
                        if (embedStatus != WatermarkingStatus.SUCCESS)
                        {
                            Console.WriteLine($"Watermark embedding failed: {embedStatus}");
                            return;
                        }

                        // Save the drawing to PDF
                        PdfOptions pdfOptions = new PdfOptions();
                        cadImage.Save(outputPath, pdfOptions);
                    }

                    // Reload the saved PDF to validate the watermark
                    using (Image loadedPdf = Image.Load(outputPath))
                    {
                        // Set the expected text for validation
                        loadedPdf.WatermarkGuardService.TextToValidate = watermarkText;

                        // Perform validation
                        WatermarkingStatus validationStatus = loadedPdf.WatermarkGuardService.ValidationStatus();
                        if (validationStatus == WatermarkingStatus.SUCCESS)
                        {
                            Console.WriteLine("Blind watermark validation succeeded.");
                        }
                        else
                        {
                            Console.WriteLine($"Blind watermark validation failed: {validationStatus}");
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