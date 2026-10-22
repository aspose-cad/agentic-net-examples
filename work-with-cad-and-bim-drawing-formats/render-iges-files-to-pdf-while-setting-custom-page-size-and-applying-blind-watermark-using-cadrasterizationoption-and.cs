using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
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
                // Input IGES file path
                string inputPath = "input.iges";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Output PDF file path
                string outputPdfPath = "output.pdf";

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPdfPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the IGES file as a CadImage
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Apply blind watermark (text) before saving
                    cadImage.WatermarkGuardService.TextToEmbed = "Confidential";

                    // Configure vector rasterization options with custom page size (8.5 x 11 inches)
                    var vectorOptions = new CadRasterizationOptions
                    {
                        PageWidth = 612f,   // 8.5 inches * 72 DPI
                        PageHeight = 792f   // 11 inches * 72 DPI
                    };

                    // Set up PDF save options
                    var pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = vectorOptions
                    };

                    // Save the CAD drawing to PDF
                    cadImage.Save(outputPdfPath, pdfOptions);

                    // Check embedding status
                    WatermarkingStatus embedStatus = cadImage.WatermarkGuardService.EmbeddingStatus();
                    Console.WriteLine($"Embedding status: {embedStatus}");
                }

                // Validate the watermark by reloading the original CAD file
                using (Image validationImage = Image.Load(inputPath))
                {
                    // Set the expected watermark text for validation
                    validationImage.WatermarkGuardService.TextToValidate = "Confidential";

                    // Perform validation
                    WatermarkingStatus validationStatus = validationImage.WatermarkGuardService.ValidationStatus();
                    Console.WriteLine($"Validation status: {validationStatus}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}