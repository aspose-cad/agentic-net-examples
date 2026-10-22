using Aspose.CAD.FileFormats.Cad.CadConsts;
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
            // Paths
            string sourceFbxPath = "input.fbx";
            string watermarkedDxfPath = "watermarked.dxf";
            string outputPdfPath = "output.pdf";

            // Ensure source file exists
            if (!File.Exists(sourceFbxPath))
            {
                Console.WriteLine($"Source file not found: {sourceFbxPath}");
                return;
            }

            // Ensure output directories exist
            Directory.CreateDirectory(Path.GetDirectoryName(watermarkedDxfPath) ?? ".");
            Directory.CreateDirectory(Path.GetDirectoryName(outputPdfPath) ?? ".");

            // Blind watermark text
            string watermarkText = "Confidential";

            // Load FBX and embed watermark
            try
            {
                using (Image loadedImage = Image.Load(sourceFbxPath))
                {
                    CadImage cadImage = (CadImage)loadedImage;

                    // Embed blind watermark
                    cadImage.WatermarkGuardService.TextToEmbed = watermarkText;

                    // Check embedding status
                    WatermarkingStatus embedStatus = cadImage.WatermarkGuardService.EmbeddingStatus();
                    if (embedStatus != WatermarkingStatus.SUCCESS)
                    {
                        Console.WriteLine($"Watermark embedding failed with status: {embedStatus}");
                        return;
                    }

                    // Save as DXF (binary format)
                    DxfOptions dxfOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert,
                        DxfFileFormat = CadFileFormat.Binary
                    };
                    cadImage.Save(watermarkedDxfPath, dxfOptions);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during loading or watermark embedding: {ex.Message}");
                return;
            }

            // Reload DXF and validate watermark
            try
            {
                using (Image reloadedImage = Image.Load(watermarkedDxfPath))
                {
                    CadImage reloadedCad = (CadImage)reloadedImage;

                    // Set expected watermark text for validation
                    reloadedCad.WatermarkGuardService.TextToValidate = watermarkText;

                    // Perform validation
                    WatermarkingStatus validationStatus = reloadedCad.WatermarkGuardService.ValidationStatus();
                    if (validationStatus == WatermarkingStatus.SUCCESS)
                    {
                        Console.WriteLine("Watermark validation succeeded.");
                    }
                    else
                    {
                        Console.WriteLine($"Watermark validation failed with status: {validationStatus}");
                    }

                    // Export to PDF, capturing any errors
                    PdfOptions pdfOptions = new PdfOptions();
                    try
                    {
                        reloadedCad.Save(outputPdfPath, pdfOptions);
                        Console.WriteLine($"PDF export completed successfully: {outputPdfPath}");
                    }
                    catch (Exception exPdf)
                    {
                        Console.WriteLine($"Error during PDF export: {exPdf.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during DXF reload or validation: {ex.Message}");
            }
        }
    }
}