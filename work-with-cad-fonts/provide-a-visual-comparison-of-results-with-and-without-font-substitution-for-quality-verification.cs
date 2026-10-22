using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class FontSubstitutionComparison
{
    static void Main()
    {
        // Path to the source CAD file
        string sourcePath = "sample.dwg";

        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        // Output paths
        string outPathNoSub = "output_without_font_substitution.png";
        string outPathWithSub = "output_with_font_substitution.png";

        // Ensure output directories exist
        string outDirNoSub = Path.GetDirectoryName(outPathNoSub);
        if (!string.IsNullOrEmpty(outDirNoSub))
            Directory.CreateDirectory(outDirNoSub);

        string outDirWithSub = Path.GetDirectoryName(outPathWithSub);
        if (!string.IsNullOrEmpty(outDirWithSub))
            Directory.CreateDirectory(outDirWithSub);

        try
        {
            // Load the CAD file as CadImage
            using (CadImage cadImage = (CadImage)Image.Load(sourcePath))
            {
                // Configure rasterization options with high quality for text, hatch and arc
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    AutomaticLayoutsScaling = true,
                    Quality = new RasterizationQuality
                    {
                        Text = RasterizationQualityValue.High,
                        Hatch = RasterizationQualityValue.High,
                        Arc = RasterizationQualityValue.High
                    }
                };

                // -------------------------------------------------
                // Render without font substitution (default font)
                // -------------------------------------------------
                var pngOptionsNoSub = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };
                cadImage.Save(outPathNoSub, pngOptionsNoSub);

                // -------------------------------------------------
                // Render with font substitution (e.g., substitute missing fonts with Arial)
                // -------------------------------------------------
                cadImage.DefaultFont = "Arial";

                var pngOptionsWithSub = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };
                cadImage.Save(outPathWithSub, pngOptionsWithSub);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}