using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.WatermarkGuard;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputPath = "output.dwg";

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // List all layers in the drawing
                Console.WriteLine("Layers in the drawing:");
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    Console.WriteLine("- " + layer.Name);
                }

                // Apply a blind watermark to the drawing
                IWatermarkGuardService watermarkService = cadImage.WatermarkGuardService;
                watermarkService.TextToEmbed = "SECURE_MARK";

                // Check embedding status
                WatermarkingStatus embedStatus = watermarkService.EmbeddingStatus();
                if (embedStatus != WatermarkingStatus.SUCCESS)
                {
                    Console.WriteLine("Watermark embedding failed: " + embedStatus);
                }

                // Save the drawing preserving editable objects
                DxfOptions saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };
                cadImage.Save(outputPath);
            }

            // Reload the saved drawing and validate the watermark
            using (Image savedImage = Image.Load(outputPath))
            {
                CadImage savedCad = (CadImage)savedImage;
                IWatermarkGuardService validateService = savedCad.WatermarkGuardService;
                validateService.TextToValidate = "SECURE_MARK";

                WatermarkingStatus validationStatus = validateService.ValidationStatus();
                if (validationStatus == WatermarkingStatus.SUCCESS)
                {
                    Console.WriteLine("Watermark validation succeeded.");
                }
                else
                {
                    Console.WriteLine("Watermark validation failed: " + validationStatus);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}