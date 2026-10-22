using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.WatermarkGuard;

namespace CadPltToPdfWatermark
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                if (args.Length < 2)
                {
                    Console.Error.WriteLine("Usage: CadPltToPdfWatermark <input.plt> <output.pdf>");
                    return;
                }

                string inputPath = args[0];
                string outputPath = args[1];

                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file does not exist: {inputPath}");
                    return;
                }

                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the PLT CAD file
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Apply blind watermark (text)
                    cadImage.WatermarkGuardService.TextToEmbed = "Confidential";

                    // Set up rasterization options for PDF rendering
                    var rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1200,
                        PageHeight = 800,
                        DrawType = CadDrawTypeMode.UseObjectColor,
                        Layouts = cadImage.Layouts.Keys.ToArray()
                    };

                    // Configure PDF options
                    var pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save to PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                // Attempt to validate watermark (not supported for PDF output)
                try
                {
                    using (Image loadedImage = Image.Load(outputPath))
                    {
                        if (loadedImage is CadImage loadedCadImage)
                        {
                            loadedCadImage.WatermarkGuardService.TextToValidate = "Confidential";
                            WatermarkingStatus validationStatus = loadedCadImage.WatermarkGuardService.ValidationStatus();

                            if (validationStatus == WatermarkingStatus.SUCCESS)
                            {
                                Console.WriteLine("Watermark validation succeeded.");
                            }
                            else
                            {
                                Console.WriteLine($"Watermark validation result: {validationStatus}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Loaded file is not a CAD image; blind watermark validation is not available for PDF output.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Watermark validation could not be performed: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}