using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.WatermarkGuard;

namespace CadWatermarkExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Paths for input, intermediate CAD, and final PDF files
                string inputFbxPath = "input.fbx";
                string watermarkedCadPath = "watermarked.dxf";
                string outputPdfPath = "output.pdf";

                // Verify input file exists
                if (!File.Exists(inputFbxPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputFbxPath}");
                    return;
                }

                // Ensure output directories exist
                string watermarkedDir = Path.GetDirectoryName(watermarkedCadPath);
                if (!string.IsNullOrEmpty(watermarkedDir) && !Directory.Exists(watermarkedDir))
                {
                    Directory.CreateDirectory(watermarkedDir);
                }

                string pdfDir = Path.GetDirectoryName(outputPdfPath);
                if (!string.IsNullOrEmpty(pdfDir) && !Directory.Exists(pdfDir))
                {
                    Directory.CreateDirectory(pdfDir);
                }

                // Load the FBX drawing
                using (Image fbxImage = Image.Load(inputFbxPath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)fbxImage;

                    // ----- Adjust CAD properties (example: set a custom property) -----
                    // Example placeholder: no specific property modification required for this demo.

                    // ----- Apply blind watermark (text) -----
                    IWatermarkGuardService watermarkService = cadImage.WatermarkGuardService;
                    watermarkService.TextToEmbed = "Confidential";

                    // Save to a CAD format (DXF) preserving editable objects
                    DxfOptions dxfOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert,
                        DxfFileFormat = Aspose.CAD.FileFormats.Cad.CadConsts.CadFileFormat.Binary
                    };
                    cadImage.Save(watermarkedCadPath, dxfOptions);

                    // Verify embedding status
                    WatermarkingStatus embedStatus = watermarkService.EmbeddingStatus();
                    if (embedStatus != WatermarkingStatus.SUCCESS)
                    {
                        Console.WriteLine($"Watermark embedding failed: {embedStatus}");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Watermark embedded successfully.");
                    }
                }

                // Reload the watermarked CAD file to validate the watermark
                using (Image watermarkedImage = Image.Load(watermarkedCadPath))
                {
                    CadImage watermarkedCad = (CadImage)watermarkedImage;

                    // Set the expected watermark text for validation
                    IWatermarkGuardService validationService = watermarkedCad.WatermarkGuardService;
                    validationService.TextToValidate = "Confidential";

                    // Perform validation
                    WatermarkingStatus validationStatus = validationService.ValidationStatus();
                    if (validationStatus != WatermarkingStatus.SUCCESS)
                    {
                        Console.WriteLine($"Watermark validation failed: {validationStatus}");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Watermark validated successfully.");
                    }

                    // Export the validated CAD to PDF
                    PdfOptions pdfOptions = new PdfOptions();
                    watermarkedCad.Save(outputPdfPath, pdfOptions);
                    Console.WriteLine($"PDF exported successfully to '{outputPdfPath}'.");
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