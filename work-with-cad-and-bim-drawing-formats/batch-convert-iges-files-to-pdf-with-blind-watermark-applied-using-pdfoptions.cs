using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

namespace CadBatchConvertWithWatermark
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Folder containing IGES files
                string inputFolder = @"C:\Cad\Input";
                // Folder where PDF files will be saved
                string outputFolder = @"C:\Cad\Output";

                // Ensure output folder exists
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                // Watermark text to embed
                const string watermarkText = "Confidential";

                // Process each IGES file in the input folder
                string[] igesFiles = Directory.GetFiles(inputFolder, "*.iges");
                foreach (string igesPath in igesFiles)
                {
                    if (!File.Exists(igesPath))
                    {
                        Console.WriteLine($"Input file not found: {igesPath}");
                        continue;
                    }

                    // Load the IGES file
                    using (Image cadImage = Image.Load(igesPath))
                    {
                        // Cast to CadImage for CAD-specific features
                        CadImage cad = cadImage as CadImage;
                        if (cad == null)
                        {
                            Console.WriteLine($"Failed to load CAD image from {igesPath}");
                            continue;
                        }

                        // Embed blind watermark text
                        cad.WatermarkGuardService.TextToEmbed = watermarkText;

                        // Prepare PDF save options
                        PdfOptions pdfOptions = new PdfOptions();

                        // NOTE: VectorRasterizationOptions is abstract and no concrete implementation is available
                        // in the current SDK for direct instantiation. The property is left at its default value.

                        // Determine output PDF path
                        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(igesPath);
                        string pdfPath = Path.Combine(outputFolder, fileNameWithoutExt + ".pdf");

                        // Save the CAD image as PDF with the embedded watermark
                        cad.Save(pdfPath, pdfOptions);
                    }

                    // Validate the watermark after reloading the saved PDF
                    try
                    {
                        using (Image savedImage = Image.Load(Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(igesPath) + ".pdf")))
                        {
                            CadImage savedCad = savedImage as CadImage;
                            if (savedCad != null)
                            {
                                savedCad.WatermarkGuardService.TextToValidate = watermarkText;
                                WatermarkingStatus validationStatus = savedCad.WatermarkGuardService.ValidationStatus();

                                if (validationStatus == WatermarkingStatus.SUCCESS)
                                {
                                    Console.WriteLine($"Watermark validation SUCCESS for {Path.GetFileName(igesPath)}");
                                }
                                else
                                {
                                    Console.WriteLine($"Watermark validation {validationStatus} for {Path.GetFileName(igesPath)}");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Unable to validate watermark for {Path.GetFileName(igesPath)} (unsupported format for validation).");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error during watermark validation for {Path.GetFileName(igesPath)}: {ex.Message}");
                    }
                }

                Console.WriteLine("Batch conversion completed.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}