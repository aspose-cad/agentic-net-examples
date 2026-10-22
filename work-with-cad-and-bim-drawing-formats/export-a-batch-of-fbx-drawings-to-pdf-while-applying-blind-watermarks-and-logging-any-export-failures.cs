using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

namespace BatchFbxToPdfWithWatermark
{
    class Program
    {
        static void Main(string[] args)
        {
            // Folder containing FBX files
            string inputFolder = @"C:\InputFbx";
            // Folder where PDF files will be saved
            string outputFolder = @"C:\OutputPdf";

            // Guard against missing input folder
            if (!Directory.Exists(inputFolder))
            {
                Console.WriteLine($"[ERROR] Input folder does not exist: {inputFolder}");
                return;
            }

            // Ensure output folder exists
            Directory.CreateDirectory(outputFolder);

            // Blind watermark text to embed
            const string watermarkText = "Confidential";

            // Process each FBX file in the input folder
            foreach (string fbxPath in Directory.GetFiles(inputFolder, "*.fbx"))
            {
                // Guard against missing input file
                if (!File.Exists(fbxPath))
                {
                    Console.WriteLine($"[SKIP] Input file not found: {fbxPath}");
                    continue;
                }

                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fbxPath);
                string pdfPath = Path.Combine(outputFolder, fileNameWithoutExt + ".pdf");

                try
                {
                    // Load the FBX drawing as a CadImage
                    using (CadImage cadImage = (CadImage)Image.Load(fbxPath))
                    {
                        // Embed blind watermark text
                        cadImage.WatermarkGuardService.TextToEmbed = watermarkText;

                        // Save to PDF
                        PdfOptions pdfOptions = new PdfOptions();
                        cadImage.Save(pdfPath, pdfOptions);
                    }

                    // Attempt to reload the saved PDF for watermark validation
                    Image loadedImage = null;
                    try
                    {
                        loadedImage = Image.Load(pdfPath);
                    }
                    catch (Exception loadEx)
                    {
                        Console.WriteLine($"[WARN] Unable to load PDF for validation: {pdfPath}. Reason: {loadEx.Message}");
                    }

                    if (loadedImage is CadImage loadedPdf)
                    {
                        // Set expected watermark text for validation
                        loadedPdf.WatermarkGuardService.TextToValidate = watermarkText;

                        // Check validation status
                        WatermarkingStatus validationStatus = loadedPdf.WatermarkGuardService.ValidationStatus();

                        if (validationStatus == WatermarkingStatus.SUCCESS)
                        {
                            Console.WriteLine($"[SUCCESS] {fbxPath} exported to PDF with valid watermark.");
                        }
                        else
                        {
                            Console.WriteLine($"[FAILURE] {fbxPath} exported, but watermark validation returned {validationStatus}.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[WARN] Loaded file is not a CadImage; watermark validation skipped for {pdfPath}.");
                    }
                }
                catch (Exception ex)
                {
                    // Log any export failures
                    Console.WriteLine($"[ERROR] Failed to process {fbxPath}: {ex.Message}");
                }
            }

            Console.WriteLine("Batch processing completed.");
        }
    }
}