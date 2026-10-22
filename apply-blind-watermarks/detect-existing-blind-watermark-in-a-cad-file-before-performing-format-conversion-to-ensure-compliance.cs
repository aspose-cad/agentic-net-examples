using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.WatermarkGuard;
using Aspose.CAD.ImageOptions;

namespace CadWatermarkCheck
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Input CAD file path
                    string inputPath = "input.dwg";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Expected blind watermark text to validate
                    string expectedWatermarkText = "Confidential";

                    // Load the CAD file
                    using (Image image = Image.Load(inputPath))
                    {
                        // Cast to CadImage to access CAD-specific members
                        CadImage cadImage = (CadImage)image;

                        // Access the watermark guard service
                        IWatermarkGuardService watermarkService = cadImage.WatermarkGuardService;

                        // Set the expected watermark text for validation
                        watermarkService.TextToValidate = expectedWatermarkText;

                        // Perform validation
                        WatermarkingStatus validationStatus = watermarkService.ValidationStatus();

                        // Report validation result
                        if (validationStatus == WatermarkingStatus.SUCCESS)
                        {
                            Console.WriteLine("Blind watermark validation succeeded.");
                        }
                        else
                        {
                            Console.WriteLine($"Blind watermark validation failed: {validationStatus}");
                            // Depending on compliance policy, you might abort conversion here
                            // For demonstration, we continue only if validation succeeded
                            return;
                        }

                        // Define output PDF path
                        string outputPath = "output.pdf";

                        // Set PDF save options
                        PdfOptions pdfOptions = new PdfOptions();

                        // Save the CAD image as PDF
                        cadImage.Save(outputPath, pdfOptions);
                        Console.WriteLine($"CAD file successfully converted to PDF: {outputPath}");
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