using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Path to the source CAD file
        string inputPath = "input.dwg";

        // Load the CAD file with error handling
        Image cadImage;
        try
        {
            cadImage = Image.Load(inputPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load CAD file '{inputPath}': {ex.Message}");
            return;
        }

        using (cadImage)
        {
            // Cast to CadImage to access CAD‑specific members if needed
            CadImage cad = (CadImage)cadImage;

            // -------------------------------------------------
            // 1. Automatic sizing (default behavior)
            // -------------------------------------------------
            string autoOutputPath = "output_auto.png";
            try
            {
                EnsureDirectoryExists(autoOutputPath);
                PngOptions autoOptions = new PngOptions();
                cad.Save(autoOutputPath, autoOptions);
                Console.WriteLine($"Automatic sizing image saved to '{autoOutputPath}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save automatic sizing image: {ex.Message}");
            }

            // -------------------------------------------------
            // 2. Manual dimension configuration
            // -------------------------------------------------
            string manualOutputPath = "output_manual.png";
            try
            {
                EnsureDirectoryExists(manualOutputPath);
                PngOptions manualOptions = new PngOptions();

                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1200,   // Desired width
                    PageHeight = 800    // Desired height
                    // No Layout property in current API version
                };

                manualOptions.VectorRasterizationOptions = rasterOptions;
                cad.Save(manualOutputPath, manualOptions);
                Console.WriteLine($"Manual dimension image saved to '{manualOutputPath}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save manual dimension image: {ex.Message}");
            }
        }
    }

    private static void EnsureDirectoryExists(string filePath)
    {
        string directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}
