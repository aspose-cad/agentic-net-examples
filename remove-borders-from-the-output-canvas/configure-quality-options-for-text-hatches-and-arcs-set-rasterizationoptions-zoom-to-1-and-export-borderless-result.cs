using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Expect input CAD file path and output PDF file path.
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: <program> <input-cad-file> <output-pdf-file>");
            return;
        }

        string inputPath = args[0];
        string outputPath = args[1];

        // Verify input file exists.
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists.
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD document.
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options.
                var rasterizationOptions = new CadRasterizationOptions
                {
                    // Set zoom to 1 (no scaling).
                    Zoom = 1,

                    // Remove borders by setting zero margins.
                    Margins = new Margins { Left = 0, Right = 0, Top = 0, Bottom = 0 }

                    // Note: The current Aspose.CAD SDK does not expose TextQuality,
                    // HatchQuality, or ArcQuality properties. These quality settings
                    // cannot be configured directly.
                };

                // Prepare PDF export options.
                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterizationOptions
                };

                // Save the borderless PDF.
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Export completed: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during processing: {ex.Message}");
        }
    }
}