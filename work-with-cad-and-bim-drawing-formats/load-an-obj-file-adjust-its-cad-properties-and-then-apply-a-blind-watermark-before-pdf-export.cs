using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.WatermarkGuard;

class Program
{
    static void Main()
    {
        try
        {
                // Paths for input OBJ, intermediate CAD with watermark, and final PDF
                string inputObjPath = "model.obj";
                string cadWithWatermarkPath = "model_with_watermark.dxf";
                string pdfOutputPath = "model.pdf";

        
                    string outputDir = Path.GetDirectoryName(pdfOutputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }
        // Load the OBJ file
                using (Image image = Image.Load(inputObjPath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Example adjustment of CAD properties (placeholder for real adjustments)
                    foreach (var layout in cadImage.Layouts.Values)
                    {
                        // No specific property change in this example
                    }

                    // Apply a blind text watermark
                    IWatermarkGuardService watermarkService = cadImage.WatermarkGuardService;
                    watermarkService.TextToEmbed = "Confidential";

                    // Save to a native CAD format (DXF) preserving editable objects
                    DxfOptions dxfOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };
                    cadImage.Save(cadWithWatermarkPath, dxfOptions);

                    // Report embedding status
                    WatermarkingStatus embedStatus = watermarkService.EmbeddingStatus();
                    Console.WriteLine($"Embedding status: {embedStatus}");

                    // Reload the saved CAD file to validate the watermark
                    using (Image reloaded = Image.Load(cadWithWatermarkPath))
                    {
                        CadImage reloadedCad = (CadImage)reloaded;
                        IWatermarkGuardService validationService = reloadedCad.WatermarkGuardService;
                        validationService.TextToValidate = "Confidential";

                        WatermarkingStatus validationStatus = validationService.ValidationStatus();
                        Console.WriteLine($"Validation status: {validationStatus}");
                    }

                    // Export the CAD image (with embedded watermark) to PDF
                    PdfOptions pdfOptions = new PdfOptions();
                    cadImage.Save(pdfOutputPath, pdfOptions);
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}