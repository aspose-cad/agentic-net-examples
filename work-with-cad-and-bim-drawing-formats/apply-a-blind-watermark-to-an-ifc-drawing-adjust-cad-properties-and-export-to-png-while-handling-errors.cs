using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

namespace CadWatermarkExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputIfcPath = "input.ifc";
            string watermarkedIfcPath = "watermarked.ifc";
            string outputPngPath = "output.png";

            // Verify input file exists
            if (!File.Exists(inputIfcPath))
            {
                Console.WriteLine($"Input file not found: {inputIfcPath}");
                return;
            }

            // Ensure output directories exist
            string watermarkedDir = Path.GetDirectoryName(watermarkedIfcPath);
            if (!string.IsNullOrEmpty(watermarkedDir))
                Directory.CreateDirectory(watermarkedDir);

            string pngDir = Path.GetDirectoryName(outputPngPath);
            if (!string.IsNullOrEmpty(pngDir))
                Directory.CreateDirectory(pngDir);

            try
            {
                // Load the original IFC drawing
                using (Image loadedImage = Image.Load(inputIfcPath))
                {
                    CadImage cadImage = (CadImage)loadedImage;

                    // ----- Apply blind watermark -----
                    cadImage.WatermarkGuardService.TextToEmbed = "Confidential";

                    // Save the CAD file with the embedded watermark
                    DxfOptions dxfSaveOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };
                    cadImage.Save(watermarkedIfcPath, dxfSaveOptions);
                }

                // Reload the saved CAD file to validate the watermark
                using (Image reloadedImage = Image.Load(watermarkedIfcPath))
                {
                    CadImage reloadedCad = (CadImage)reloadedImage;

                    // Set the expected text for validation
                    reloadedCad.WatermarkGuardService.TextToValidate = "Confidential";

                    // Perform validation
                    WatermarkingStatus validationStatus = reloadedCad.WatermarkGuardService.ValidationStatus();

                    // Report validation result
                    switch (validationStatus)
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
                            Console.WriteLine("Watermark validation returned an unexpected status.");
                            break;
                    }

                    // ----- Adjust CAD properties (example) -----
                    // Change the default font as a simple property adjustment
                    reloadedCad.DefaultFont = "Arial";

                    // ----- Export to PNG -----
                    PngOptions pngOptions = new PngOptions();
                    reloadedCad.Save(outputPngPath, pngOptions);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}