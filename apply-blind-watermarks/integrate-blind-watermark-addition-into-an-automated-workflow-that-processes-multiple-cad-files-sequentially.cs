using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.WatermarkGuard;

class Program
{
    static void Main()
    {
        try
        {
                // Define input and output directories
                string inputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "InputCAD");
                string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "OutputCAD");

                // Ensure output directory exists
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Verify input directory exists
                if (!Directory.Exists(inputDirectory))
                {
                    Console.WriteLine("Input directory not found: " + inputDirectory);
                    return;
                }

                // Get all DWG files in the input directory
                string[] cadFiles = Directory.GetFiles(inputDirectory, "*.dwg");
                if (cadFiles.Length == 0)
                {
                    Console.WriteLine("No DWG files found in the input directory.");
                    return;
                }

                // Process each CAD file sequentially
                foreach (string cadFilePath in cadFiles)
                {
                    // Load the CAD image
                    using (Image cadImage = Image.Load(cadFilePath))
                    {
                        // Embed a blind text watermark
                        cadImage.WatermarkGuardService.TextToEmbed = "Confidential";

                        // Prepare DXF save options (preserve editable objects)
                        DxfOptions dxfOptions = new DxfOptions
                        {
                            OutputMode = CadOutputMode.Convert
                        };

                        // Define output file path (same name with .dxf extension)
                        string outputFileName = Path.GetFileNameWithoutExtension(cadFilePath) + ".dxf";
                        string outputFilePath = Path.Combine(outputDirectory, outputFileName);

                        // Save the CAD image with the embedded watermark
                        cadImage.Save(outputFilePath, dxfOptions);
                    }

                    // Reload the saved file to validate the watermark
                    using (Image savedImage = Image.Load(Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(cadFilePath) + ".dxf")))
                    {
                        // Set the expected watermark text for validation
                        savedImage.WatermarkGuardService.TextToValidate = "Confidential";

                        // Perform validation
                        WatermarkingStatus validationStatus = savedImage.WatermarkGuardService.ValidationStatus();

                        // Report validation result
                        if (validationStatus == WatermarkingStatus.SUCCESS)
                        {
                            Console.WriteLine($"Watermark validation SUCCESS for file: {Path.GetFileName(cadFilePath)}");
                        }
                        else
                        {
                            Console.WriteLine($"Watermark validation {validationStatus} for file: {Path.GetFileName(cadFilePath)}");
                        }
                    }
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}