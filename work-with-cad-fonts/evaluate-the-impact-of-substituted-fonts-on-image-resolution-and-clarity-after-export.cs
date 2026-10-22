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
            string inputPath = "input.dwg";
            string outputPathDefault = "output_default.png";
            string outputPathSubstituted = "output_substituted.png";

            // Ensure output directories exist
            Directory.CreateDirectory(Path.GetDirectoryName(outputPathDefault) ?? ".");
            Directory.CreateDirectory(Path.GetDirectoryName(outputPathSubstituted) ?? ".");

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Export with the original font settings
                CadRasterizationOptions rasterOptionsDefault = new CadRasterizationOptions
                {
                    // Optional: set page size or other options here
                };

                PngOptions pngOptionsDefault = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptionsDefault
                };

                cadImage.Save(outputPathDefault, pngOptionsDefault);

                // Substitute missing fonts by setting the DefaultFont property
                cadImage.DefaultFont = "Arial";

                // Export again with the same raster options
                CadRasterizationOptions rasterOptionsSubstituted = new CadRasterizationOptions
                {
                    // Same settings as above
                };

                PngOptions pngOptionsSubstituted = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptionsSubstituted
                };

                cadImage.Save(outputPathSubstituted, pngOptionsSubstituted);
            }

            Console.WriteLine("Export completed. Compare the two PNG files to evaluate the impact of font substitution on resolution and clarity.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}