using System;
using System.IO;
using System.Threading.Tasks;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static async Task Main(string[] args)
    {
        // Input STEP file path
        string inputPath = "input.stp";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Output PDF file path
        string outputPath = "output.pdf";

        // Ensure output directory exists
        try
        {
            string? outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to create output directory: {ex.Message}");
            return;
        }

        // Load the STEP file and export to PDF asynchronously
        try
        {
            using (Image cadImage = Image.Load(inputPath))
            {
                PdfOptions pdfOptions = new PdfOptions();

                await Task.Run(() =>
                {
                    try
                    {
                        cadImage.Save(outputPath, pdfOptions);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error during PDF save: {ex.Message}");
                        throw;
                    }
                });
            }

            Console.WriteLine("Export completed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}