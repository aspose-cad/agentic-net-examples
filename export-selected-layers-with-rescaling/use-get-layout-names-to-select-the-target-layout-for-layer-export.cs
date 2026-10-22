using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Expected arguments: input CAD file, output directory, layout name to export
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: <input.cad> <outputDirectory> <layoutName>");
            return;
        }

        string inputPath = args[0];
        string outputDir = args[1];
        string targetLayoutName = args[2];

        // Verify that the input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure the output directory exists
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD image
            using (var cadImage = (CadImage)Image.Load(inputPath))
            {
                // Retrieve layout names from the image
                var layoutNames = cadImage.Layouts.Keys.ToArray();

                Console.WriteLine("Available layouts:");
                foreach (var name in layoutNames)
                {
                    Console.WriteLine($" - {name}");
                }

                // Find the layout that matches the requested name (case‑insensitive)
                var targetLayout = layoutNames
                    .FirstOrDefault(n => string.Equals(n, targetLayoutName, StringComparison.OrdinalIgnoreCase));

                if (targetLayout == null)
                {
                    Console.WriteLine($"Layout '{targetLayoutName}' not found.");
                    return;
                }

                // Common rasterization options for all export formats
                var rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,
                    Layouts = new[] { targetLayout }
                };

                // ---------- PDF export ----------
                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };
                string pdfPath = Path.Combine(outputDir,
                    $"{Path.GetFileNameWithoutExtension(inputPath)}_{targetLayout}.pdf");
                cadImage.Save(pdfPath, pdfOptions);
                Console.WriteLine($"PDF saved to {pdfPath}");

                // ---------- SVG export ----------
                var svgOptions = new SvgOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };
                string svgPath = Path.Combine(outputDir,
                    $"{Path.GetFileNameWithoutExtension(inputPath)}_{targetLayout}.svg");
                cadImage.Save(svgPath, svgOptions);
                Console.WriteLine($"SVG saved to {svgPath}");

                // ---------- PNG export ----------
                var pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };
                string pngPath = Path.Combine(outputDir,
                    $"{Path.GetFileNameWithoutExtension(inputPath)}_{targetLayout}.png");
                cadImage.Save(pngPath, pngOptions);
                Console.WriteLine($"PNG saved to {pngPath}");
            }
        }
        catch (Exception ex)
        {
            // Report any errors that occur during processing
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}