using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.WatermarkGuard;

class Program
{
    static void Main(string[] args)
    {
        // Input CAD file path
        string inputPath = "input.dwg";

        // Output directory and PDF file path
        string outputDir = "output";
        string outputPdfPath = Path.Combine(outputDir, "output.pdf");

        // Verify that the input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure the output directory exists
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cadImage = (CadImage)image;

                // Embed a blind watermark text
                cadImage.WatermarkGuardService.TextToEmbed = "Confidential";

                // Prepare PDF save options
                PdfOptions pdfOptions = new PdfOptions();

                // Save the CAD drawing as PDF with the embedded watermark
                cadImage.Save(outputPdfPath, pdfOptions);

                // Report embedding status
                WatermarkingStatus embedStatus = cadImage.WatermarkGuardService.EmbeddingStatus();
                Console.WriteLine($"Watermark embedding status: {embedStatus}");
            }

            // Validate the watermark by reloading the CAD file
            using (Image validationImage = Image.Load(inputPath))
            {
                CadImage validationCad = (CadImage)validationImage;
                validationCad.WatermarkGuardService.TextToValidate = "Confidential";

                WatermarkingStatus validationStatus = validationCad.WatermarkGuardService.ValidationStatus();
                Console.WriteLine($"Watermark validation status: {validationStatus}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}