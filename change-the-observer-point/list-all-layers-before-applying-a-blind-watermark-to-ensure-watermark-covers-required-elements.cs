using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.ImageOptions;
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

            string outputPath = "output.dxf";

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // List all layers in the drawing
                Console.WriteLine("Layers in the drawing:");
                foreach (CadLayerTable layerRecord in cadImage.Layers)
                {
                    Console.WriteLine("- " + layerRecord.Name);
                }

                // Apply a blind watermark (text only)
                IWatermarkGuardService watermarkService = cadImage.WatermarkGuardService;
                watermarkService.TextToEmbed = "Confidential";

                // Save the drawing preserving editable objects
                DxfOptions dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };
                cadImage.Save(outputPath, dxfOptions);
            }

            // Reload the saved file and validate the watermark
            using (Image savedImage = Image.Load(outputPath))
            {
                CadImage savedCadImage = (CadImage)savedImage;
                IWatermarkGuardService validationService = savedCadImage.WatermarkGuardService;
                validationService.TextToValidate = "Confidential";

                WatermarkingStatus status = validationService.ValidationStatus();
                Console.WriteLine("Watermark validation status: " + status);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}