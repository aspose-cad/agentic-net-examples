using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input IFC file (replace with actual path if needed)
            string inputFilePath = "sample.ifc";

            if (!File.Exists(inputFilePath))
            {
                Console.Error.WriteLine($"Input file not found: {inputFilePath}");
                return;
            }

            // Prepare output directory
            string outputDirectory = Path.Combine(Path.GetDirectoryName(inputFilePath) ?? "", "output");
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Path for the watermarked IFC file
            string watermarkedIfcPath = Path.Combine(outputDirectory, "watermarked.ifc");

            // Load the IFC drawing and embed a blind watermark
            using (Image cadImage = Image.Load(inputFilePath))
            {
                IWatermarkGuardService watermarkService = cadImage.WatermarkGuardService;
                watermarkService.TextToEmbed = "Confidential";

                WatermarkingStatus embedStatus = watermarkService.EmbeddingStatus();
                if (embedStatus != WatermarkingStatus.SUCCESS)
                {
                    Console.Error.WriteLine($"Watermark embedding failed with status: {embedStatus}");
                    return;
                }

                // Save the watermarked IFC file
                cadImage.Save(watermarkedIfcPath);
            }

            // Re‑load the saved file and validate the watermark
            using (Image reloadedImage = Image.Load(watermarkedIfcPath))
            {
                IWatermarkGuardService watermarkService = reloadedImage.WatermarkGuardService;
                watermarkService.TextToValidate = "Confidential";

                WatermarkingStatus validationStatus = watermarkService.ValidationStatus();
                if (validationStatus != WatermarkingStatus.SUCCESS)
                {
                    Console.Error.WriteLine($"Watermark validation failed with status: {validationStatus}");
                    return;
                }
            }

            // Export the watermarked IFC drawing to PNG
            string pngOutputPath = Path.Combine(outputDirectory, "result.png");
            using (Image pngSource = Image.Load(watermarkedIfcPath))
            {
                PngOptions pngOptions = new PngOptions();
                pngSource.Save(pngOutputPath, pngOptions);
            }

            Console.WriteLine("Watermark embedded, validated, and PNG exported successfully.");
            Console.WriteLine($"Watermarked IFC: {watermarkedIfcPath}");
            Console.WriteLine($"PNG image: {pngOutputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}