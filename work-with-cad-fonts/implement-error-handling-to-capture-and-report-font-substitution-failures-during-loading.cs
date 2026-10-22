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
        string inputPath = "Resources/sample.dwg";

        // Prepare output directory and file
        string outputDir = "Output";
        string outputPath = Path.Combine(outputDir, "sample.pdf");

        try
        {
            // Verify source file existence
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Source CAD file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Load the CAD image with Aspose.CAD
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD‑specific members
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Loaded image is not a CAD image.");
                    return;
                }

                // Intentionally set a non‑existent default font to trigger substitution failure
                cadImage.DefaultFont = "NonExistentFont";

                // Configure PDF export options (no rasterization needed for vector output)
                PdfOptions pdfOptions = new PdfOptions();

                // Attempt to save; any font‑related errors will be caught by the outer catch block
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"CAD file successfully saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            // Handle font substitution failures and other processing errors
            Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}