using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

class Program
{
    static void Main()
    {
        try
        {
            // Folder containing source IFC files
            string inputFolder = @"InputIfc";
            // Folder to store generated PNG files
            string outputFolder = @"OutputPng";

            if (!Directory.Exists(inputFolder))
            {
                Console.WriteLine($"Input folder does not exist: {inputFolder}");
                return;
            }

            Directory.CreateDirectory(outputFolder);

            // Text to embed as blind watermark
            const string watermarkText = "Confidential";

            // Process each IFC file in the input folder
            foreach (string ifcPath in Directory.GetFiles(inputFolder, "*.ifc"))
            {
                if (!File.Exists(ifcPath))
                {
                    Console.WriteLine($"File not found, skipping: {ifcPath}");
                    continue;
                }

                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(ifcPath);
                string pngPath = Path.Combine(outputFolder, fileNameWithoutExt + ".png");

                // Load IFC drawing
                using (CadImage cadImage = (CadImage)Image.Load(ifcPath))
                {
                    // Set watermark text to embed
                    cadImage.WatermarkGuardService.TextToEmbed = watermarkText;

                    // Check embedding status
                    WatermarkingStatus embedStatus = cadImage.WatermarkGuardService.EmbeddingStatus();
                    if (embedStatus != WatermarkingStatus.SUCCESS)
                    {
                        Console.WriteLine($"Embedding failed for {ifcPath}: {embedStatus}");
                        continue;
                    }

                    // Save to PNG
                    PngOptions pngOptions = new PngOptions();
                    cadImage.Save(pngPath, pngOptions);
                }

                if (!File.Exists(pngPath))
                {
                    Console.WriteLine($"Failed to create PNG for {ifcPath}");
                    continue;
                }

                // Reload the saved PNG to validate the watermark
                using (Image pngImage = Image.Load(pngPath))
                {
                    // Set expected watermark text for validation
                    pngImage.WatermarkGuardService.TextToValidate = watermarkText;

                    // Check validation status
                    WatermarkingStatus validationStatus = pngImage.WatermarkGuardService.ValidationStatus();
                    if (validationStatus == WatermarkingStatus.SUCCESS)
                    {
                        Console.WriteLine($"Watermark successfully validated for {pngPath}");
                    }
                    else
                    {
                        Console.WriteLine($"Watermark validation result for {pngPath}: {validationStatus}");
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