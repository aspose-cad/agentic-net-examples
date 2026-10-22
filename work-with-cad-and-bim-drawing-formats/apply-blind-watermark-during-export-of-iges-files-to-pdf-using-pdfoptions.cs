using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

namespace CadWatermarkExample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Resolve input and output paths
            string inputFileName = args.Length > 0 ? args[0] : "sample.iges";
            string inputFilePath = Path.GetFullPath(inputFileName);

            if (!File.Exists(inputFilePath))
            {
                Console.Error.WriteLine($"Input file not found: {inputFilePath}");
                return;
            }

            string outputFolder = Path.Combine(Directory.GetCurrentDirectory(), "output");
            Directory.CreateDirectory(outputFolder);
            string outputFileName = Path.GetFileNameWithoutExtension(inputFilePath) + ".pdf";
            string outputFilePath = Path.Combine(outputFolder, outputFileName);

            try
            {
                // Load the source IGES file
                using (Image sourceImage = Image.Load(inputFilePath))
                {
                    // Embed blind watermark text
                    sourceImage.WatermarkGuardService.TextToEmbed = "Confidential – Do Not Distribute";

                    // Prepare PDF export options with concrete rasterization options
                    PdfOptions pdfOptions = new PdfOptions();
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 8.5f,
                        PageHeight = 11.0f,
                        UnitType = UnitType.Inch
                    };
                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Export to PDF (watermark embedded during save)
                    sourceImage.Save(outputFilePath, pdfOptions);
                }

                // Validate the embedded watermark
                using (Image pdfImage = Image.Load(outputFilePath))
                {
                    pdfImage.WatermarkGuardService.TextToValidate = "Confidential – Do Not Distribute";
                    WatermarkingStatus validationResult = pdfImage.WatermarkGuardService.ValidationStatus();

                    switch (validationResult)
                    {
                        case WatermarkingStatus.SUCCESS:
                            Console.WriteLine("Watermark validation succeeded.");
                            break;
                        case WatermarkingStatus.FAILURE:
                            Console.WriteLine("Watermark validation failed.");
                            break;
                        case WatermarkingStatus.UNKNOWN:
                            Console.WriteLine("Watermark validation status unknown.");
                            break;
                        case WatermarkingStatus.PENDING:
                            Console.WriteLine("Watermark validation pending.");
                            break;
                        default:
                            Console.WriteLine($"Unexpected watermark validation status: {validationResult}");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}