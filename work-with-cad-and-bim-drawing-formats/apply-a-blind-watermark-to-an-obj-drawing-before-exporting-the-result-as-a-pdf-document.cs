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
                    // Paths for input OBJ and output PDF
                    string inputPath = "input.obj";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        string outputPdfPath = "watermarked_output.pdf";

                    // Load the OBJ drawing as a CadImage
                    using (Image image = Image.Load(inputPath))
                    {
                        Image cadImage = image ;
                        if (cadImage == null)
                        {
                            Console.WriteLine("Failed to load OBJ as CadImage.");
                            return;
                        }

                        // Obtain the watermark guard service
                        IWatermarkGuardService watermarkService = cadImage.WatermarkGuardService;

                        // Set the blind watermark text to embed
                        watermarkService.TextToEmbed = "Confidential";

                        // Verify embedding status
                        WatermarkingStatus embedStatus = watermarkService.EmbeddingStatus();
                        if (embedStatus == WatermarkingStatus.SUCCESS)
                        {
                            Console.WriteLine("Watermark embedding succeeded.");
                        }
                        else
                        {
                            Console.WriteLine($"Watermark embedding failed with status: {embedStatus}");
                            // Proceeding may produce an invalid watermark; continue or exit as needed
                        }

                        // Save the watermarked CAD as PDF
                        PdfOptions pdfOptions = new PdfOptions();
                        cadImage.Save(outputPdfPath, pdfOptions);
                        Console.WriteLine($"Saved watermarked PDF to: {outputPdfPath}");
                    }

                    // Reload the saved PDF to validate the blind watermark
                    using (Image loadedPdf = Image.Load(outputPdfPath))
                    {
                        CadImage pdfCadImage = loadedPdf as CadImage;
                        if (pdfCadImage == null)
                        {
                            Console.WriteLine("Failed to load PDF as CadImage for validation.");
                            return;
                        }

                        // Obtain the watermark guard service for validation
                        IWatermarkGuardService validationService = pdfCadImage.WatermarkGuardService;

                        // Set the expected watermark text to validate
                        validationService.TextToValidate = "Confidential";

                        // Check validation status
                        WatermarkingStatus validationStatus = validationService.ValidationStatus();
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
                            Console.WriteLine("Watermark validation status unknown.");
                        }
                        else if (validationStatus == WatermarkingStatus.PENDING)
                        {
                            Console.WriteLine("Watermark validation pending.");
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
            return;
        }
}
    }
}