using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadConsts;

namespace CadWatermarkUtility
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input OBJ file path
            string inputObjPath = @"C:\CAD\model.obj";

            // Text to embed as blind watermark
            string watermarkText = "Confidential-2026";

            // Temporary CAD file to validate watermark (DXF)
            string tempDxfPath = @"C:\CAD\model_watermarked.dxf";

            // Final PDF output path
            string outputPdfPath = @"C:\CAD\model_watermarked.pdf";

            // Guard input file existence
            if (!File.Exists(inputObjPath))
            {
                Console.WriteLine($"Input OBJ file not found: {inputObjPath}");
                return;
            }

            // Ensure output directories exist
            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(tempDxfPath)));
            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(outputPdfPath)));

            // Load OBJ with load options
            var loadOptions = new LoadOptions
            {
                IgnoreErrors = false
            };

            Image cadImage = null;
            try
            {
                cadImage = Image.Load(inputObjPath, loadOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while loading OBJ: {ex.Message}");
                foreach (var err in loadOptions.Errors)
                {
                    Console.WriteLine($"Load error: {err.Message} (Code: {err.RenderCode})");
                }
                return;
            }

            // Apply blind text watermark before saving
            cadImage.WatermarkGuardService.TextToEmbed = watermarkText;

            // Save to a temporary DXF to validate the blind watermark embedding
            var dxfOptions = new DxfOptions
            {
                OutputMode = CadOutputMode.Convert,
                Version = DxfOutputVersion.R12,
                DxfFileFormat = CadFileFormat.Binary
            };

            try
            {
                cadImage.Save(tempDxfPath, dxfOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while saving temporary DXF: {ex.Message}");
                cadImage.Dispose();
                return;
            }

            // Reload the saved DXF to validate the watermark
            CadImage reloadedImage = null;
            try
            {
                reloadedImage = (CadImage)Image.Load(tempDxfPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while reloading DXF for validation: {ex.Message}");
                cadImage.Dispose();
                return;
            }

            // Set expected watermark text for validation
            reloadedImage.WatermarkGuardService.TextToValidate = watermarkText;

            // Perform validation
            var validationStatus = reloadedImage.WatermarkGuardService.ValidationStatus();

            if (validationStatus == WatermarkingStatus.SUCCESS)
            {
                Console.WriteLine("Blind watermark validation succeeded.");
            }
            else
            {
                Console.WriteLine($"Blind watermark validation failed. Status: {validationStatus}");
                cadImage.Dispose();
                reloadedImage.Dispose();
                return;
            }

            // Prepare PDF export with render result callback for error handling
            var rasterOptions = new CadRasterizationOptions
            {
                UnitType = UnitType.Millimeter,
                PageWidth = 210,   // A4 width in mm
                PageHeight = 297,  // A4 height in mm
                AutomaticLayoutsScaling = true,
                RenderResult = result =>
                {
                    if (!result.IsRenderComplete)
                    {
                        Console.WriteLine("PDF rendering incomplete. Failures:");
                        foreach (var failure in result.Failures)
                        {
                            Console.WriteLine($" - {failure.Message} (Code: {failure.RenderCode})");
                        }
                    }
                }
            };

            var pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save the final PDF
            try
            {
                cadImage.Save(outputPdfPath, pdfOptions);
                Console.WriteLine($"PDF saved successfully to: {outputPdfPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while saving PDF: {ex.Message}");
            }
            finally
            {
                cadImage?.Dispose();
                reloadedImage?.Dispose();
            }
        }
    }
}