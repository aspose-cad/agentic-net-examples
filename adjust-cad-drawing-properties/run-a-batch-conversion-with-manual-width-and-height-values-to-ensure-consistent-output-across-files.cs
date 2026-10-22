using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace BatchConversionExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input folder containing DWG/DXF files
            string inputFolder = @"C:\CadInput";
            // Output folder for converted PNG files
            string outputFolder = @"C:\CadOutput";

            // Prepare output directory
            try
            {
                if (!Directory.Exists(outputFolder))
                    Directory.CreateDirectory(outputFolder);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Desired output dimensions
            int outputWidth = 800;
            int outputHeight = 600;

            string[] cadFiles;
            string[] dxfFiles;
            try
            {
                cadFiles = Directory.GetFiles(inputFolder, "*.dwg");
                dxfFiles = Directory.GetFiles(inputFolder, "*.dxf");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to enumerate CAD files: {ex.Message}");
                return;
            }

            string[] allFiles = new string[cadFiles.Length + dxfFiles.Length];
            cadFiles.CopyTo(allFiles, 0);
            dxfFiles.CopyTo(allFiles, cadFiles.Length);

            foreach (string inputPath in allFiles)
            {
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(inputPath);
                string outputPath = Path.Combine(outputFolder, fileNameWithoutExt + ".png");

                try
                {
                    // Load CAD image
                    using (Image cadImage = Image.Load(inputPath))
                    {
                        // Configure PNG options with manual width and height
                        var rasterizationOptions = new CadRasterizationOptions
                        {
                            PageWidth = outputWidth,
                            PageHeight = outputHeight
                        };

                        var pngOptions = new PngOptions
                        {
                            VectorRasterizationOptions = rasterizationOptions
                        };

                        // Save the image as PNG with specified dimensions
                        cadImage.Save(outputPath, pngOptions);
                    }

                    Console.WriteLine($"Converted '{inputPath}' to '{outputPath}' with size {outputWidth}x{outputHeight}.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to convert '{inputPath}': {ex.Message}");
                }
            }

            Console.WriteLine("Batch conversion completed.");
        }
    }
}
