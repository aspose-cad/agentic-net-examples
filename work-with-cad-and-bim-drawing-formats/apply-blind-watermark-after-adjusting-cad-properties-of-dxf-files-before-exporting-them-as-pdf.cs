using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

namespace CadWatermarkExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Expect input DXF path as first argument
            if (args.Length == 0 || string.IsNullOrWhiteSpace(args[0]))
            {
                Console.Error.WriteLine("Error: No input file path provided.");
                return;
            }

            string inputPath = args[0];

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: Input file \"{inputPath}\" does not exist.");
                return;
            }

            // Prepare output directory
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
            string outputDirParent = Path.GetDirectoryName(outputDir);
            if (!string.IsNullOrEmpty(outputDirParent) && !Directory.Exists(outputDirParent))
            {
                Directory.CreateDirectory(outputDirParent);
            }
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            string outputPdfPath = Path.Combine(outputDir, "result.pdf");

            try
            {
                // Load the DXF file
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage for CAD‑specific access
                    CadImage cadImage = (CadImage)image;

                    // Adjust CAD properties: attempt to change line color to red for all lines in model space
                    // Note: CadLine does not expose a Color property in the current Aspose.CAD API, so direct color modification is not supported.
                    foreach (var entity in cadImage.Entities)
                    {
                        if (entity is CadLine line)
                        {
                            // Color modification not available; placeholder for future implementation.
                        }
                    }

                    // Also adjust lines inside block definitions
                    foreach (var blockEntry in cadImage.BlockEntities.ValuesTyped)
                    {
                        foreach (var blockEntity in blockEntry.Entities)
                        {
                            if (blockEntity is CadLine line)
                            {
                                // Color modification not available; placeholder for future implementation.
                            }
                        }
                    }

                    // Set up blind watermark (text) before saving
                    IWatermarkGuardService watermarkService = image.WatermarkGuardService;
                    watermarkService.TextToEmbed = "Confidential";

                    // Prepare PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the modified drawing as PDF (watermark will be embedded)
                    image.Save(outputPdfPath, pdfOptions);
                }

                // Validate the blind watermark in the saved PDF
                using (Image savedPdf = Image.Load(outputPdfPath))
                {
                    IWatermarkGuardService validationService = savedPdf.WatermarkGuardService;
                    validationService.TextToValidate = "Confidential";

                    WatermarkingStatus status = validationService.ValidationStatus();

                    if (status != WatermarkingStatus.SUCCESS)
                    {
                        Console.Error.WriteLine($"Watermark validation failed: {status}");
                    }
                    else
                    {
                        Console.WriteLine("Watermark successfully embedded and validated.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}