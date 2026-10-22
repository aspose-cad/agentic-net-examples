using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

namespace CadPltToPdfWithBlindWatermark
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input PLT file path
                string inputPltPath = "input.plt";
                // Output PDF file path
                string outputPdfPath = "output.pdf";
                // Blind watermark text to embed
                string watermarkText = "Confidential";

                // Validate input file existence
                if (!File.Exists(inputPltPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPltPath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPdfPath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the PLT file as a CadImage
                using (Image cadImage = Image.Load(inputPltPath))
                {
                    // Embed blind watermark text
                    cadImage.WatermarkGuardService.TextToEmbed = watermarkText;

                    // Check embedding status
                    WatermarkingStatus embedStatus = cadImage.WatermarkGuardService.EmbeddingStatus();
                    if (embedStatus != WatermarkingStatus.SUCCESS)
                    {
                        Console.WriteLine($"Watermark embedding failed with status: {embedStatus}");
                    }

                    // Configure PDF save options with custom page size (A4: 595x842 points)
                    PdfOptions pdfOptions = new PdfOptions();
                    // CadRasterizationOptions derives from VectorRasterizationOptions
                    CadRasterizationOptions vectorOptions = new CadRasterizationOptions
                    {
                        PageWidth = 595,
                        PageHeight = 842
                    };
                    pdfOptions.VectorRasterizationOptions = vectorOptions;

                    // Save the CAD image as PDF
                    cadImage.Save(outputPdfPath, pdfOptions);
                }

                // Validate the embedded watermark by reloading the CAD file
                using (Image cadImageValidate = Image.Load(inputPltPath))
                {
                    // Set the expected watermark text for validation
                    cadImageValidate.WatermarkGuardService.TextToValidate = watermarkText;

                    // Perform validation
                    WatermarkingStatus validationStatus = cadImageValidate.WatermarkGuardService.ValidationStatus();

                    // Report validation result
                    if (validationStatus == WatermarkingStatus.SUCCESS)
                    {
                        Console.WriteLine("Blind watermark validation succeeded.");
                    }
                    else
                    {
                        Console.WriteLine($"Blind watermark validation status: {validationStatus}");
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