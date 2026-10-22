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
        string sourcePath = "sample.dwg";

        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source CAD file not found: {sourcePath}");
            return;
        }

        // Prepare output paths
        string defaultSvgPath = "output_default.svg";
        string defaultPdfPath = "output_default.pdf";
        string adjustedSvgPath = "output_adjusted.svg";
        string adjustedPdfPath = "output_adjusted.pdf";

        // Ensure output directories exist
        CreateDirectoryIfNeeded(defaultSvgPath);
        CreateDirectoryIfNeeded(defaultPdfPath);
        CreateDirectoryIfNeeded(adjustedSvgPath);
        CreateDirectoryIfNeeded(adjustedPdfPath);

        try
        {
            // Load the CAD image
            using (CadImage cadImage = (CadImage)Image.Load(sourcePath))
            {
                // -------------------------
                // Default rasterization settings
                // -------------------------
                CadRasterizationOptions defaultRasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,
                    LineScale = 1.0f
                };

                // SVG export with default settings
                SvgOptions defaultSvgOptions = new SvgOptions
                {
                    VectorRasterizationOptions = defaultRasterOptions
                };
                cadImage.Save(defaultSvgPath, defaultSvgOptions);

                // PDF export with default settings
                PdfOptions defaultPdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = defaultRasterOptions
                };
                cadImage.Save(defaultPdfPath, defaultPdfOptions);

                // -------------------------
                // Adjusted rasterization settings
                // -------------------------
                CadRasterizationOptions adjustedRasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,
                    LineScale = 0.5f,
                    Margins = new Margins
                    {
                        Left = 10,
                        Right = 10,
                        Top = 10,
                        Bottom = 10
                    }
                };

                // SVG export with adjusted settings
                SvgOptions adjustedSvgOptions = new SvgOptions
                {
                    VectorRasterizationOptions = adjustedRasterOptions
                };
                cadImage.Save(adjustedSvgPath, adjustedSvgOptions);

                // PDF export with adjusted settings
                PdfOptions adjustedPdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = adjustedRasterOptions
                };
                cadImage.Save(adjustedPdfPath, adjustedPdfOptions);
            }

            // -------------------------
            // Report file sizes
            // -------------------------
            FileInfo defaultSvgInfo = new FileInfo(defaultSvgPath);
            FileInfo adjustedSvgInfo = new FileInfo(adjustedSvgPath);
            FileInfo defaultPdfInfo = new FileInfo(defaultPdfPath);
            FileInfo adjustedPdfInfo = new FileInfo(adjustedPdfPath);

            Console.WriteLine($"Default SVG size: {defaultSvgInfo.Length} bytes");
            Console.WriteLine($"Adjusted SVG size: {adjustedSvgInfo.Length} bytes");
            Console.WriteLine($"Default PDF size: {defaultPdfInfo.Length} bytes");
            Console.WriteLine($"Adjusted PDF size: {adjustedPdfInfo.Length} bytes");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }

    private static void CreateDirectoryIfNeeded(string filePath)
    {
        string directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}