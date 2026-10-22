using System;
using System.Diagnostics;
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
            // Array of input CAD file paths (replace with actual paths)
            string[] cadFiles = new string[]
            {
                "drawing1.dwg",
                "drawing2.dxf",
                "drawing3.dwg"
            };

            // Output folder for rendered images
            string outputFolder = "RenderedImages";
            Directory.CreateDirectory(outputFolder);

            // Stopwatch for overall benchmark
            Stopwatch totalStopwatch = new Stopwatch();
            totalStopwatch.Start();

            foreach (string cadFilePath in cadFiles)
            {
                if (!File.Exists(cadFilePath))
                {
                    Console.WriteLine($"File not found: {cadFilePath}");
                    continue;
                }

                // Measure rendering time for each file
                Stopwatch fileStopwatch = new Stopwatch();
                fileStopwatch.Start();

                // Load CAD drawing
                using (Image cadImage = Image.Load(cadFilePath))
                {
                    // Cast to CadImage to access CAD-specific members if needed
                    CadImage cadSpecificImage = cadImage as CadImage;

                    // Prepare rasterization options
                    CadRasterizationOptions rasterizationOptions = new CadRasterizationOptions
                    {
                        // Example: set page size (optional)
                        PageWidth = 1600,
                        PageHeight = 1200
                    };

                    // Prepare PNG save options
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterizationOptions
                    };

                    // Define output file path
                    string outputFileName = Path.GetFileNameWithoutExtension(cadFilePath) + ".png";
                    string outputPath = Path.Combine(outputFolder, outputFileName);

                    // Save rendered image
                    cadImage.Save(outputPath, pngOptions);
                }

                fileStopwatch.Stop();
                Console.WriteLine($"Rendered {Path.GetFileName(cadFilePath)} in {fileStopwatch.ElapsedMilliseconds} ms");
            }

            totalStopwatch.Stop();
            Console.WriteLine($"Total rendering time for {cadFiles.Length} files: {totalStopwatch.ElapsedMilliseconds} ms");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}