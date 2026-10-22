using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.WatermarkGuard;

namespace BlindWatermarkFbxToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Paths to source FBX files
                string[] fbxFiles = new string[]
                {
                    @"C:\Drawings\model1.fbx",
                    @"C:\Drawings\model2.fbx",
                    @"C:\Drawings\model3.fbx"
                };

                // Desired blind watermark text
                const string watermarkText = "Confidential";

                // Output folder for PDFs
                string outputFolder = @"C:\Drawings\PdfOutput";
                Directory.CreateDirectory(outputFolder);

                foreach (string fbxPath in fbxFiles)
                {
                    if (!File.Exists(fbxPath))
                    {
                        Console.WriteLine($"File not found: {fbxPath}");
                        continue;
                    }

                    // Load the FBX drawing as a CadImage
                    using (CadImage cadImage = (CadImage)Image.Load(fbxPath))
                    {
                        // Embed blind watermark text
                        IWatermarkGuardService watermarkService = cadImage.WatermarkGuardService;
                        watermarkService.TextToEmbed = watermarkText;

                        // Check embedding status
                        WatermarkingStatus embedStatus = watermarkService.EmbeddingStatus();
                        if (embedStatus != WatermarkingStatus.SUCCESS)
                        {
                            Console.WriteLine($"Embedding failed for '{fbxPath}'. Status: {embedStatus}");
                            continue;
                        }

                        // Prepare PDF export options
                        PdfOptions pdfOptions = new PdfOptions();

                        // Determine output PDF path
                        string pdfFileName = Path.GetFileNameWithoutExtension(fbxPath) + ".pdf";
                        string pdfPath = Path.Combine(outputFolder, pdfFileName);

                        // Save the drawing as PDF
                        cadImage.Save(pdfPath, pdfOptions);
                        Console.WriteLine($"Saved PDF: {pdfPath}");

                        // Reload the original CAD file to validate the watermark
                        using (CadImage validationImage = (CadImage)Image.Load(fbxPath))
                        {
                            IWatermarkGuardService validationService = validationImage.WatermarkGuardService;
                            validationService.TextToValidate = watermarkText;

                            WatermarkingStatus validationStatus = validationService.ValidationStatus();
                            if (validationStatus == WatermarkingStatus.SUCCESS)
                            {
                                Console.WriteLine($"Watermark validation succeeded for '{fbxPath}'.");
                            }
                            else
                            {
                                Console.WriteLine($"Watermark validation failed for '{fbxPath}'. Status: {validationStatus}");
                            }
                        }
                    }
                }

                Console.WriteLine("Processing completed.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}