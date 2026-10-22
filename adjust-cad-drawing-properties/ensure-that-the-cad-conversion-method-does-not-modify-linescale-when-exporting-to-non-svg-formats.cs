using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Example usage: convert input.dwg to output.png
        string inputPath = "input.dwg";
        string outputPath = "output.png";
        ConvertCad(inputPath, outputPath);
    }

    static void ConvertCad(string inputFile, string outputFile)
    {
        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"Input file not found: {inputFile}");
            return;
        }

        string outputDir = Path.GetDirectoryName(outputFile);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            try
            {
                Directory.CreateDirectory(outputDir);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }
        }

        try
        {
            using (Image cadImage = Image.Load(inputFile))
            {
                string ext = Path.GetExtension(outputFile).ToLowerInvariant();

                if (ext == ".svg")
                {
                    var svgRasterOptions = new CadRasterizationOptions
                    {
                        LineScale = 2.0f,
                        PageWidth = 1600,
                        PageHeight = 1200
                    };
                    var svgSaveOptions = new SvgOptions
                    {
                        VectorRasterizationOptions = svgRasterOptions
                    };
                    cadImage.Save(outputFile, svgSaveOptions);
                }
                else if (ext == ".png")
                {
                    var pngRasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200
                        // LineScale not set for non‑SVG formats
                    };
                    var pngSaveOptions = new PngOptions
                    {
                        VectorRasterizationOptions = pngRasterOptions
                    };
                    cadImage.Save(outputFile, pngSaveOptions);
                }
                else if (ext == ".pdf")
                {
                    var pdfRasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200
                        // LineScale not set for non‑SVG formats
                    };
                    var pdfSaveOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = pdfRasterOptions
                    };
                    cadImage.Save(outputFile, pdfSaveOptions);
                }
                else
                {
                    var fallbackRasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200
                    };
                    var fallbackSaveOptions = new PngOptions
                    {
                        VectorRasterizationOptions = fallbackRasterOptions
                    };
                    cadImage.Save(outputFile, fallbackSaveOptions);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}
