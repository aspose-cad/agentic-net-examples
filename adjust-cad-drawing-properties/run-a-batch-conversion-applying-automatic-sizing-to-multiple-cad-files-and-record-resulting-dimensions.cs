using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output directories
            string inputFolder = "InputCadFiles";
            string outputFolder = "OutputImages";

            // Verify input folder exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder '{inputFolder}' does not exist.");
                return;
            }

            // Ensure the output directory exists
            Directory.CreateDirectory(outputFolder);

            // Get all DWG files in the input folder
            string[] cadFiles = Directory.GetFiles(inputFolder, "*.dwg");

            if (cadFiles.Length == 0)
            {
                Console.WriteLine("No DWG files found to process.");
                return;
            }

            foreach (string cadPath in cadFiles)
            {
                try
                {
                    // Load the CAD file
                    using (Image image = Image.Load(cadPath))
                    {
                        CadImage cadImage = (CadImage)image;

                        // Record original dimensions (if available)
                        int originalWidth = cadImage.Width;
                        int originalHeight = cadImage.Height;

                        Console.WriteLine($"Processing '{Path.GetFileName(cadPath)}' - Original size: {originalWidth}x{originalHeight}");

                        // Configure rasterization options for automatic sizing
                        CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                        {
                            PageWidth = 0,   // Auto width
                            PageHeight = 0,  // Auto height
                            AutomaticLayoutsScaling = true
                        };

                        // Set PNG save options using the rasterization settings
                        PngOptions pngOptions = new PngOptions
                        {
                            VectorRasterizationOptions = rasterOptions
                        };

                        // Define output file path
                        string outputPath = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(cadPath) + ".png");

                        // Save the CAD drawing as PNG with automatic sizing
                        cadImage.Save(outputPath, pngOptions);

                        // Since System.Drawing is not allowed, we cannot load the PNG to get its dimensions.
                        // Instead, we report that the file was saved. If needed, dimensions can be inferred from the CAD layout.
                        Console.WriteLine($"Saved PNG to '{outputPath}'.");
                    }
                }
                catch (Exception fileEx)
                {
                    Console.Error.WriteLine($"Failed to process '{Path.GetFileName(cadPath)}': {fileEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}
