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
            string sourcePath = "input.dwg";
            string outputPath = "output.pdf";
            string watermarkText = "Confidential";

            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (Image image = Image.Load(sourcePath))
            {
                if (image is CadImage cadImage)
                {
                    cadImage.WatermarkGuardService.TextToEmbed = watermarkText;

                    PdfOptions pdfOptions = new PdfOptions();
                    cadImage.Save(outputPath, pdfOptions);
                }
                else
                {
                    Console.Error.WriteLine("Loaded image is not a CadImage.");
                    return;
                }
            }

            if (File.Exists(outputPath))
            {
                using (Image savedImage = Image.Load(outputPath))
                {
                    if (savedImage is CadImage loadedCad)
                    {
                        loadedCad.WatermarkGuardService.TextToValidate = watermarkText;
                        WatermarkingStatus status = loadedCad.WatermarkGuardService.ValidationStatus();
                        Console.WriteLine($"Watermark validation status: {status}");
                    }
                    else
                    {
                        Console.WriteLine("Loaded file is not a CadImage; cannot validate watermark.");
                    }
                }
            }
            else
            {
                Console.Error.WriteLine($"Output file not found: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}