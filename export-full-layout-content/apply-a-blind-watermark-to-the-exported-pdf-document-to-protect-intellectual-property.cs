using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Input CAD file path
        string inputCadPath = "input.dwg";

        // Output paths
        string outputPdfPath = "output.pdf";
        string outputDxfPath = "output.dxf";

        if (!File.Exists(inputCadPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputCadPath}");
            return;
        }

        try
        {
            // Load the CAD image
            using (Image cadImage = Image.Load(inputCadPath))
            {
                // Apply a blind text watermark before saving
                IWatermarkGuardService watermarkService = cadImage.WatermarkGuardService;
                watermarkService.TextToEmbed = "Confidential";

                // Save as PDF
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(outputPdfPath, pdfOptions);

                // Save as DXF preserving editable objects
                DxfOptions dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };
                cadImage.Save(outputDxfPath, dxfOptions);
            }

            // Reload the saved DXF to validate the watermark
            using (Image reloadedCad = Image.Load(outputDxfPath))
            {
                IWatermarkGuardService validationService = reloadedCad.WatermarkGuardService;
                validationService.TextToValidate = "Confidential";

                WatermarkingStatus status = validationService.ValidationStatus();

                Console.WriteLine($"Watermark validation status: {status}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}