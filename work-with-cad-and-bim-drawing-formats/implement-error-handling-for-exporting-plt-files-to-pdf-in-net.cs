using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "sample.plt";
        string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        string? outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Configure load options to ignore errors during loading
            LoadOptions loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            // Load the PLT file
            Image cadImage = Image.Load(inputPath, loadOptions);

            // Check for any loading warnings/errors
            if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
            {
                Console.Error.WriteLine("Warnings during loading:");
                foreach (var error in loadOptions.Errors)
                {
                    Console.Error.WriteLine($"- {error.Message} (Code: {error.RenderCode})");
                }
            }

            // Set PDF export options (optional rasterization settings can be added)
            PdfOptions pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = new CadRasterizationOptions()
            };

            // Export to PDF
            cadImage.Save(outputPath, pdfOptions);

            Console.WriteLine($"Successfully exported '{inputPath}' to PDF at '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}