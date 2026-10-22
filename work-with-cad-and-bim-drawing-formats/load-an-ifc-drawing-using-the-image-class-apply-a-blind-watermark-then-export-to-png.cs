using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

namespace CadBlindWatermarkExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input IFC file path (place the file in the same folder as the executable)
                string inputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "input.ifc");
                if (!File.Exists(inputFilePath))
                {
                    Console.WriteLine("Input file not found: " + inputFilePath);
                    return;
                }

                // Load the IFC drawing
                Image cadImage = Image.Load(inputFilePath);

                // Apply blind watermark (text based)
                IWatermarkGuardService watermarkService = cadImage.WatermarkGuardService;
                watermarkService.TextToEmbed = "Confidential";

                // Verify embedding status
                WatermarkingStatus embedStatus = watermarkService.EmbeddingStatus();
                if (embedStatus != WatermarkingStatus.SUCCESS)
                {
                    Console.WriteLine("Watermark embedding failed with status: " + embedStatus);
                    return;
                }
                else
                {
                    Console.WriteLine("Watermark embedded successfully.");
                }

                // Set up rasterization options for PNG export
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.UnitType = UnitType.Centimenter; // SDK spelling
                rasterOptions.LineScale = 0.5f;

                // Configure PNG options
                PngOptions pngOptions = new PngOptions();
                pngOptions.VectorRasterizationOptions = rasterOptions;

                // Define output PNG file path
                string outputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output.png");
                string outputDir = Path.GetDirectoryName(outputFilePath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the image as PNG
                cadImage.Save(outputFilePath, pngOptions);

                Console.WriteLine("PNG file saved to: " + outputFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}