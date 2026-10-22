using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

class BatchIgesToPdfWithBlindWatermark
{
    static void Main()
    {
        // Input folder containing IGES files
        string inputFolder = @"C:\Cad\Input";
        // Output folder for generated PDFs
        string outputFolder = @"C:\Cad\Output";

        // Verify input folder exists
        if (!Directory.Exists(inputFolder))
        {
            Console.WriteLine($"Input folder does not exist: {inputFolder}");
            return;
        }

        // Ensure output folder exists
        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        // Watermark text to embed
        const string watermarkText = "Confidential";

        // Custom page size (in points; 1 point = 1/72 inch)
        const float pageWidth = 595f;   // A4 width ~8.27in *72
        const float pageHeight = 842f;  // A4 height ~11.69in *72

        // Process each IGES file in the input folder
        foreach (string filePath in Directory.GetFiles(inputFolder, "*.iges"))
        {
            try
            {
                // Load the IGES file as a CadImage
                using (CadImage cadImage = (CadImage)Image.Load(filePath))
                {
                    // Embed blind watermark text
                    cadImage.WatermarkGuardService.TextToEmbed = watermarkText;

                    // Verify embedding succeeded
                    WatermarkingStatus embedStatus = cadImage.WatermarkGuardService.EmbeddingStatus();
                    if (embedStatus != WatermarkingStatus.SUCCESS)
                    {
                        Console.WriteLine($"Embedding failed for '{Path.GetFileName(filePath)}'. Status: {embedStatus}");
                        continue;
                    }

                    // Prepare PDF export options with custom page size
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = new CadRasterizationOptions
                        {
                            PageWidth = pageWidth,
                            PageHeight = pageHeight,
                            // Use layout names for proper layout handling
                            Layouts = cadImage.Layouts.Keys.ToArray()
                        }
                    };

                    // Define output PDF path
                    string outputPdfPath = Path.Combine(outputFolder,
                        Path.GetFileNameWithoutExtension(filePath) + ".pdf");

                    // Save the CAD image as PDF
                    cadImage.Save(outputPdfPath, pdfOptions);
                    Console.WriteLine($"Converted '{Path.GetFileName(filePath)}' to PDF successfully.");

                    // Validate the blind watermark after saving
                    cadImage.WatermarkGuardService.TextToValidate = watermarkText;
                    WatermarkingStatus validationStatus = cadImage.WatermarkGuardService.ValidationStatus();

                    if (validationStatus == WatermarkingStatus.SUCCESS)
                        Console.WriteLine($"Watermark validation succeeded for '{Path.GetFileName(filePath)}'.");
                    else
                        Console.WriteLine($"Watermark validation failed for '{Path.GetFileName(filePath)}'. Status: {validationStatus}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing '{Path.GetFileName(filePath)}': {ex.Message}");
            }
        }
    }
}