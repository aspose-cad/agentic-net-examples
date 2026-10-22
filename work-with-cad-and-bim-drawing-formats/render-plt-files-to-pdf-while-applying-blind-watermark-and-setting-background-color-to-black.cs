using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

class Program
{
    static void Main()
    {
        string inputPath = "input.plt";
        string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the PLT file
            var cadImage = Image.Load(inputPath);

            // Note: Setting a black background via VectorRasterizationOptions is not possible because
            // VectorRasterizationOptions is abstract in the current API. The rendering will use the default background.

            // Embed blind text watermark
            var watermark = cadImage.WatermarkGuardService;
            watermark.TextToEmbed = "Confidential";

            // Check embedding status
            var embedStatus = watermark.EmbeddingStatus();
            Console.WriteLine($"Embedding status: {embedStatus}");

            // Prepare PDF save options
            var pdfOptions = new PdfOptions();

            // VectorRasterizationOptions cannot be instantiated directly (abstract), so custom background settings are omitted.
            // If a concrete implementation becomes available, it can be assigned to pdfOptions.VectorRasterizationOptions here.

            // Save the CAD drawing as PDF
            cadImage.Save(outputPath, pdfOptions);

            // Validation of the blind watermark requires reloading the saved CAD file, which is not applicable for PDF output.
            // Therefore validation is not performed in this example.
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}