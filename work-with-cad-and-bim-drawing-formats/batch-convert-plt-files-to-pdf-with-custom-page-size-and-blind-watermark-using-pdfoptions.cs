using Aspose.CAD.FileFormats.Cad;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

class PltToPdfBatchConverter
{
    static void Main()
    {
        // Input folder containing PLT files
        string inputFolder = @"C:\InputPlt";
        // Output folder for generated PDFs
        string outputFolder = @"C:\OutputPdf";

        // Verify input folder exists
        if (!Directory.Exists(inputFolder))
        {
            Console.WriteLine($"Input folder does not exist: {inputFolder}");
            return;
        }

        // Create output folder if it does not exist
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }

        // Process each .plt file in the input folder
        foreach (string pltPath in Directory.GetFiles(inputFolder, "*.plt"))
        {
            try
            {
                // Load the PLT file as a CAD image
                using (Image image = Image.Load(pltPath))
                {
                    // Cast to CadImage for watermark access
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine($"Failed to cast image to CadImage: {pltPath}");
                        continue;
                    }

                    // ----- Blind watermark embedding -----
                    // Set the text to embed (blind watermark)
                    cadImage.WatermarkGuardService.TextToEmbed = "Confidential";

                    // Prepare PDF export options with custom page size (8.5 x 11 inches, 72 points per inch)
                    PdfOptions pdfOptions = new PdfOptions();
                    var rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 8.5f * 72f,
                        PageHeight = 11f * 72f,
                        UnitType = UnitType.Centimenter,
                        LineScale = 1.0f
                    };
                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Determine output PDF path
                    string pdfFileName = Path.GetFileNameWithoutExtension(pltPath) + ".pdf";
                    string pdfPath = Path.Combine(outputFolder, pdfFileName);

                    // Save the CAD image as PDF (watermark is embedded automatically)
                    cadImage.Save(pdfPath, pdfOptions);
                    Console.WriteLine($"Saved PDF: {pdfPath}");

                    // ----- Blind watermark validation -----
                    // Reload the saved PDF to validate the embedded watermark
                    using (Image reloadedImage = Image.Load(pdfPath))
                    {
                        CadImage reloadedCad = reloadedImage as CadImage;
                        if (reloadedCad == null)
                        {
                            Console.WriteLine($"Unable to load saved PDF as CadImage for validation: {pdfPath}");
                            continue;
                        }

                        // Set the expected text to validate
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
                                Console.WriteLine($"Unexpected validation status: {validationStatus}");
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file '{pltPath}': {ex.Message}");
            }
        }
    }
}