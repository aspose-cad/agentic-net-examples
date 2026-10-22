using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input directory containing IGES files
            string inputDir = @"C:\InputIGES";
            // Output directory for generated PDF files
            string outputDir = @"C:\OutputPDF";

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Verify input directory exists
            if (!Directory.Exists(inputDir))
            {
                Console.WriteLine($"Input directory does not exist: {inputDir}");
                return;
            }

            // Retrieve all IGES files from the input directory
            string[] igesFiles = Directory.GetFiles(inputDir, "*.iges");

            foreach (string igesPath in igesFiles)
            {
                if (!File.Exists(igesPath))
                {
                    Console.WriteLine($"File not found: {igesPath}");
                    continue;
                }

                // Load the IGES file as a CAD image
                using (Image cadImage = Image.Load(igesPath))
                {
                    CadImage cadImg = cadImage as CadImage;
                    if (cadImg == null)
                    {
                        Console.WriteLine($"Unable to load CAD image: {igesPath}");
                        continue;
                    }

                    // Embed a blind watermark (text) into the CAD image
                    cadImg.WatermarkGuardService.TextToEmbed = "Confidential";

                    // Verify that the watermark was embedded successfully
                    WatermarkingStatus embedStatus = cadImg.WatermarkGuardService.EmbeddingStatus();
                    if (embedStatus != WatermarkingStatus.SUCCESS)
                    {
                        Console.WriteLine($"Watermark embedding failed for {igesPath}: {embedStatus}");
                    }

                    // Configure PDF export options with high resolution (resolution properties not available in current API)
                    PdfOptions pdfOptions = new PdfOptions();

                    // Use a concrete implementation of VectorRasterizationOptions
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 2100,   // Width in points (example: A4 * 10)
                        PageHeight = 2970   // Height in points (example: A4 * 10)
                        // HorizontalResolution and VerticalResolution are not supported in this API version.
                    };

                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Determine the output PDF file path
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(igesPath);
                    string pdfPath = Path.Combine(outputDir, fileNameWithoutExt + ".pdf");

                    // Save the CAD image as a PDF file
                    cadImg.Save(pdfPath, pdfOptions);

                    // Reload the saved PDF to validate the blind watermark (if supported)
                    using (Image loadedPdf = Image.Load(pdfPath))
                    {
                        CadImage loadedCad = loadedPdf as CadImage;
                        if (loadedCad != null)
                        {
                            loadedCad.WatermarkGuardService.TextToValidate = "Confidential";
                            WatermarkingStatus validationStatus = loadedCad.WatermarkGuardService.ValidationStatus();
                            Console.WriteLine($"Validation status for {pdfPath}: {validationStatus}");
                        }
                        else
                        {
                            Console.WriteLine($"Unable to validate watermark for {pdfPath}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}