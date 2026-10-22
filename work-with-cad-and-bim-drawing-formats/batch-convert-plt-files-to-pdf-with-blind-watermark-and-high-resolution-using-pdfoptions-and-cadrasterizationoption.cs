using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.WatermarkGuard;

class Program
{
    static void Main()
    {
        // Input and output directories
        string inputDir = @"C:\InputPlt";
        string outputDir = @"C:\OutputPdf";

        // Blind watermark text to embed
        string watermarkText = "Confidential";

        // Verify input directory exists
        if (!Directory.Exists(inputDir))
        {
            Console.WriteLine($"Input directory does not exist: {inputDir}");
            return;
        }

        // Ensure output directory exists
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Process each PLT file in the input folder
        foreach (string filePath in Directory.GetFiles(inputDir, "*.plt"))
        {
            try
            {
                // Load the PLT file as a CadImage
                using (CadImage cadImage = (CadImage)Image.Load(filePath))
                {
                    // Embed a blind watermark (text only)
                    cadImage.WatermarkGuardService.TextToEmbed = watermarkText;
                    WatermarkingStatus embedStatus = cadImage.WatermarkGuardService.EmbeddingStatus();

                    // Check embedding result
                    if (embedStatus != WatermarkingStatus.SUCCESS)
                    {
                        Console.WriteLine($"Failed to embed watermark in {Path.GetFileName(filePath)}. Status: {embedStatus}");
                        continue;
                    }

                    // Configure high‑resolution rasterization options
                    var rasterOptions = new CadRasterizationOptions
                    {
                        // High resolution can be approximated by larger page dimensions
                        PageWidth = 2480,   // e.g., A4 at 300 DPI width in pixels
                        PageHeight = 3508,  // e.g., A4 at 300 DPI height in pixels
                        Layouts = cadImage.Layouts.Keys.ToArray()
                    };

                    // Configure PDF export options
                    var pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Build output PDF file path
                    string outputFile = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(filePath) + ".pdf");

                    // Save the CAD image as PDF with the specified options
                    cadImage.Save(outputFile, pdfOptions);

                    // Validate the watermark by reloading the original CAD file
                    using (CadImage reloaded = (CadImage)Image.Load(filePath))
                    {
                        reloaded.WatermarkGuardService.TextToValidate = watermarkText;
                        WatermarkingStatus validationStatus = reloaded.WatermarkGuardService.ValidationStatus();
                        Console.WriteLine($"File: {Path.GetFileName(filePath)} - Watermark validation: {validationStatus}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing {Path.GetFileName(filePath)}: {ex.Message}");
            }
        }
    }
}