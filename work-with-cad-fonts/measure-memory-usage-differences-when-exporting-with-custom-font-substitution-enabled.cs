using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

namespace CadMemoryUsageExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dwg";
            string outputDefaultPath = "output_default.png";
            string outputCustomFontPath = "output_customfont.png";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Ensure output directories exist
                var defaultDir = Path.GetDirectoryName(outputDefaultPath);
                if (!string.IsNullOrEmpty(defaultDir))
                    Directory.CreateDirectory(defaultDir);
                var customDir = Path.GetDirectoryName(outputCustomFontPath);
                if (!string.IsNullOrEmpty(customDir))
                    Directory.CreateDirectory(customDir);

                // Load CAD image
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Prepare rasterization options (common for both exports)
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200,
                        Layouts = new[] { cadImage.Layouts.Keys.FirstOrDefault() ?? string.Empty }
                    };

                    // Export with default font substitution (no custom font)
                    PngOptions pngOptionsDefault = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    long memoryBeforeDefault = GC.GetTotalMemory(true);
                    cadImage.Save(outputDefaultPath, pngOptionsDefault);
                    long memoryAfterDefault = GC.GetTotalMemory(true);
                    long memoryUsedDefault = memoryAfterDefault - memoryBeforeDefault;

                    // Export with custom font substitution enabled
                    cadImage.DefaultFont = "Arial";

                    // Reuse same raster options
                    PngOptions pngOptionsCustom = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    long memoryBeforeCustom = GC.GetTotalMemory(true);
                    cadImage.Save(outputCustomFontPath, pngOptionsCustom);
                    long memoryAfterCustom = GC.GetTotalMemory(true);
                    long memoryUsedCustom = memoryAfterCustom - memoryBeforeCustom;

                    // Output results
                    Console.WriteLine($"Memory used without custom font: {memoryUsedDefault} bytes");
                    Console.WriteLine($"Memory used with custom font (\"Arial\"): {memoryUsedCustom} bytes");
                    Console.WriteLine($"Difference: {memoryUsedCustom - memoryUsedDefault} bytes");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}