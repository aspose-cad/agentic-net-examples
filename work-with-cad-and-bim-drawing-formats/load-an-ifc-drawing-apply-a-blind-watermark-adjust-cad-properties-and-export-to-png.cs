using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.WatermarkGuard;

namespace CadWatermarkExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Paths for input IFC, intermediate watermarked IFC, and final PNG
                string inputIfcPath = "input.ifc";
                string watermarkedIfcPath = "watermarked.ifc";
                string outputPngPath = "output.png";

                // Verify input file exists
                if (!File.Exists(inputIfcPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputIfcPath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPngPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the IFC drawing
                using (Image ifcImage = Image.Load(inputIfcPath))
                {
                    CadImage cadImage = (CadImage)ifcImage;

                    // ----- Apply blind watermark (text) -----
                    IWatermarkGuardService watermarkService = cadImage.WatermarkGuardService;
                    watermarkService.TextToEmbed = "Confidential";

                    // Save the IFC with the embedded watermark
                    cadImage.Save(watermarkedIfcPath);

                    // Verify embedding status
                    WatermarkingStatus embedStatus = watermarkService.EmbeddingStatus();
                    Console.WriteLine("Embedding Status: " + embedStatus);
                    if (embedStatus != WatermarkingStatus.SUCCESS)
                    {
                        Console.WriteLine("Failed to embed watermark. Exiting.");
                        return;
                    }

                    // ----- Adjust CAD properties (example: change line color to red) -----
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        if (entity.TypeName == CadEntityTypeName.LINE)
                        {
                            // Change line color to red using ColorId (1 = red)
                            entity.ColorId = 1;
                        }
                    }

                    // Save again after property adjustment
                    cadImage.Save(watermarkedIfcPath);
                }

                // Reload the watermarked IFC to validate the watermark
                using (Image reloadedImage = Image.Load(watermarkedIfcPath))
                {
                    CadImage reloadedCad = (CadImage)reloadedImage;
                    IWatermarkGuardService validationService = reloadedCad.WatermarkGuardService;
                    validationService.TextToValidate = "Confidential";

                    WatermarkingStatus validationStatus = validationService.ValidationStatus();
                    Console.WriteLine("Validation Status: " + validationStatus);
                    if (validationStatus != WatermarkingStatus.SUCCESS)
                    {
                        Console.WriteLine("Watermark validation failed. Exiting.");
                        return;
                    }
                }

                // Export the watermarked IFC to PNG
                using (Image finalIfc = Image.Load(watermarkedIfcPath))
                {
                    PngOptions pngOptions = new PngOptions();
                    finalIfc.Save(outputPngPath, pngOptions);
                    Console.WriteLine("Exported PNG saved to: " + outputPngPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}