using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.WatermarkGuard;

namespace CadWatermarkExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Paths for input IFC file and output PNG image
            string inputIfcPath = "input.ifc";
            string outputPngPath = "output.png";

            // Verify input file exists
            if (!File.Exists(inputIfcPath))
            {
                Console.WriteLine($"Input file not found: {inputIfcPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPngPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the IFC drawing
            using (Image loadedImage = Image.Load(inputIfcPath))
            {
                // Cast to CadImage to access watermark functionality
                Image cadImage = loadedImage ;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to load the file as a CAD image.");
                    return;
                }

                // Set the blind watermark text to embed
                cadImage.WatermarkGuardService.TextToEmbed = "Confidential";

                // (Optional) Embed an image watermark
                // using (FileStream imgStream = File.OpenRead("watermark.png"))
                // {
                //     cadImage.WatermarkGuardService.ImageToEmbed = imgStream;
                // }

                // Save the CAD image as PNG
                PngOptions pngOptions = new PngOptions();
                try
                {
                    cadImage.Save(outputPngPath, pngOptions);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Export to PNG failed: {ex.Message}");
                    return;
                }

                // Verify embedding status
                WatermarkingStatus embedStatus = cadImage.WatermarkGuardService.EmbeddingStatus();
                if (embedStatus == WatermarkingStatus.SUCCESS)
                {
                    Console.WriteLine("Blind watermark embedding succeeded.");
                }
                else
                {
                    Console.WriteLine($"Blind watermark embedding status: {embedStatus}");
                }
            }
        }
    }
}