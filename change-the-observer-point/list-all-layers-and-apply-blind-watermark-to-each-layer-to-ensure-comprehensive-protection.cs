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

            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                Console.WriteLine("Layers in the drawing:");
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    Console.WriteLine("- " + layer.Name);
                }

                IWatermarkGuardService watermarkService = cadImage.WatermarkGuardService;
                watermarkService.TextToEmbed = "Confidential";

                DxfOptions dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };
                cadImage.Save(outputPath, dxfOptions);

                WatermarkingStatus embedStatus = watermarkService.EmbeddingStatus();
                Console.WriteLine("Watermark embedding status: " + embedStatus);
            }

            using (Image savedImage = Image.Load(outputPath))
            {
                CadImage savedCadImage = (CadImage)savedImage;
                IWatermarkGuardService savedWatermarkService = savedCadImage.WatermarkGuardService;
                savedWatermarkService.TextToValidate = "Confidential";

                WatermarkingStatus validationStatus = savedWatermarkService.ValidationStatus();
                Console.WriteLine("Watermark validation status: " + validationStatus);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}