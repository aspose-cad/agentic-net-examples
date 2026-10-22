using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;
using Aspose.CAD.FileFormats.Cad;

namespace PltToPdfBatchWatermark
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Folder containing source PLT files
                string inputFolder = @"C:\InputPltFiles";
                // Folder where PDF files will be saved
                string outputFolder = @"C:\OutputPdfFiles";

                // Verify input folder exists
                if (!Directory.Exists(inputFolder))
                {
                    Console.Error.WriteLine($"Input folder does not exist: {inputFolder}");
                    return;
                }

                // Ensure output folder exists
                Directory.CreateDirectory(outputFolder);

                // Text to embed as a blind watermark
                string watermarkText = "Confidential";

                // Process each PLT file in the input folder
                string[] pltFiles = Directory.GetFiles(inputFolder, "*.plt");
                foreach (string pltFilePath in pltFiles)
                {
                    if (!File.Exists(pltFilePath))
                    {
                        Console.WriteLine($"File not found, skipping: {pltFilePath}");
                        continue;
                    }

                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(pltFilePath);
                    string outputPdfPath = Path.Combine(outputFolder, fileNameWithoutExt + ".pdf");

                    // Load the PLT file
                    using (Image cadImage = Image.Load(pltFilePath))
                    {
                        // Embed blind watermark text before saving
                        cadImage.WatermarkGuardService.TextToEmbed = watermarkText;

                        // Prepare PDF save options with a concrete VectorRasterizationOptions implementation
                        PdfOptions pdfOptions = new PdfOptions
                        {
                            VectorRasterizationOptions = new CadRasterizationOptions()
                        };

                        // Save as PDF
                        cadImage.Save(outputPdfPath, pdfOptions);
                    }

                    // Validate the embedded watermark by reloading the saved PDF
                    using (Image pdfImage = Image.Load(outputPdfPath))
                    {
                        // Set the expected watermark text for validation
                        pdfImage.WatermarkGuardService.TextToValidate = watermarkText;

                        // Perform validation
                        WatermarkingStatus validationStatus = pdfImage.WatermarkGuardService.ValidationStatus();

                        // Report validation result
                        if (validationStatus == WatermarkingStatus.SUCCESS)
                        {
                            Console.WriteLine($"Watermark validation SUCCESS for file: {fileNameWithoutExt}.pdf");
                        }
                        else
                        {
                            Console.WriteLine($"Watermark validation {validationStatus} for file: {fileNameWithoutExt}.pdf");
                        }
                    }
                }

                Console.WriteLine("Batch conversion and watermark validation completed.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}