using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Dwf;

namespace CadLayerExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input DWF file path
                string inputFilePath = "input.dwf";

                // Output JPEG file path
                string outputFilePath = "output.jpg";

                // Verify input file exists
                if (!File.Exists(inputFilePath))
                {
                    Console.WriteLine($"Input file not found: {inputFilePath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputFilePath);
                if (!string.IsNullOrEmpty(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the DWF drawing
                using (Image image = Image.Load(inputFilePath))
                {
                    if (image is DwfImage dwfImage)
                    {
                        Console.WriteLine("Layers in the drawing:");
                        // Extract layer names
                        var layerNames = dwfImage.Layers.GetLayersNames();
                        foreach (var name in layerNames)
                        {
                            Console.WriteLine($"- {name}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed to load DWF image.");
                        return;
                    }

                    // Set JPEG export options with rasterization settings
                    var rasterOptions = new CadRasterizationOptions
                    {
                        BackgroundColor = Aspose.CAD.Color.White
                    };

                    var jpegOptions = new JpegOptions
                    {
                        Quality = 90,
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the drawing as JPEG
                    image.Save(outputFilePath, jpegOptions);
                }

                // Record and display the JPEG file size
                FileInfo jpegInfo = new FileInfo(outputFilePath);
                long fileSizeBytes = jpegInfo.Length;
                Console.WriteLine($"JPEG file saved to '{outputFilePath}' with size {fileSizeBytes} bytes.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}