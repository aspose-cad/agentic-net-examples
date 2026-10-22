using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class BlindWatermarkExport
{
    static void Main()
    {
        // Paths
        string inputPath = "input.ifc";
        string pngPath = "output.png";
        string dxfPath = "watermarked.dxf";

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(pngPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }
        outputDir = Path.GetDirectoryName(dxfPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // Load IFC with error handling
        var loadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        Image cadImage;
        try
        {
            cadImage = Image.Load(inputPath, loadOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load CAD file: {ex.Message}");
            foreach (var err in loadOptions.Errors)
                Console.WriteLine($"Load error: {err.Message}");
            return;
        }

        // Apply blind watermark (text)
        cadImage.WatermarkGuardService.TextToEmbed = "Confidential";

        // Save CAD (DXF) to preserve watermark for later validation
        var dxfOptions = new DxfOptions
        {
            OutputMode = CadOutputMode.Convert,
            DxfFileFormat = CadFileFormat.Binary
        };
        try
        {
            cadImage.Save(dxfPath, dxfOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save DXF: {ex.Message}");
            return;
        }

        // Reload saved CAD to validate watermark
        var validateLoadOptions = new LoadOptions
        {
            IgnoreErrors = true
        };

        CadImage reloaded;
        try
        {
            reloaded = (CadImage)Image.Load(dxfPath, validateLoadOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to reload watermarked CAD: {ex.Message}");
            foreach (var err in validateLoadOptions.Errors)
                Console.WriteLine($"Load error: {err.Message}");
            return;
        }

        // Validate watermark
        reloaded.WatermarkGuardService.TextToValidate = "Confidential";
        var validationStatus = reloaded.WatermarkGuardService.ValidationStatus();

        if (validationStatus == WatermarkingStatus.SUCCESS)
            Console.WriteLine("Watermark validation succeeded.");
        else
            Console.WriteLine($"Watermark validation failed: {validationStatus}");

        // Export to PNG with render callback handling
        var pngOptions = new PngOptions
        {
            VectorRasterizationOptions = new CadRasterizationOptions
            {
                PageWidth = 1600,
                PageHeight = 1200,
                UnitType = UnitType.Point,
                RenderResult = result =>
                {
                    if (result.IsRenderComplete)
                    {
                        Console.WriteLine("Render completed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Render incomplete. Failures:");
                        foreach (var failure in result.Failures)
                        {
                            Console.WriteLine($" - {failure.Message}");
                        }
                    }
                }
            }
        };

        // Save PNG
        try
        {
            cadImage.Save(pngPath, pngOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to export PNG: {ex.Message}");
        }
    }
}