using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace PenOptionsDemo
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Input CAD file – replace with an existing file path.
                string inputPath = "sample.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input CAD file not found: {inputPath}");
                    return;
                }

                // Prepare output directory.
                string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
                Directory.CreateDirectory(outputDir);
                string outputPath = Path.Combine(outputDir, "TransparentPen.png");

                // Load CAD image.
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // Enumerate top‑level entities to demonstrate access.
                    foreach (var entity in cadImage.Entities)
                    {
                        Console.WriteLine($"{entity.GetType().Name} on layer '{entity.LayerName}'");
                    }

                    // Configure PNG export with transparent pen (draw) color.
                    var pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = new CadRasterizationOptions
                        {
                            // Set transparent draw color to make pen transparent.
                            DrawColor = Aspose.CAD.Color.Transparent,
                            // Optional: also make background transparent.
                            BackgroundColor = Aspose.CAD.Color.Transparent,
                            // Use the original image dimensions.
                            PageWidth = image.Width,
                            PageHeight = image.Height
                        }
                    };

                    // Save the image.
                    cadImage.Save(outputPath, pngOptions);
                }

                Console.WriteLine($"Export completed: {Path.GetFullPath(outputPath)}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}