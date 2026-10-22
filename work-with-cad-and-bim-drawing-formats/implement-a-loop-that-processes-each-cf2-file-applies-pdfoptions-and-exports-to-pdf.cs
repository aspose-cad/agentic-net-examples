using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadToPdfBatch
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder input and output directories
                string inputDirectory = @"C:\InputCF2Files";
                string outputDirectory = @"C:\OutputPdfFiles";

                // Verify directories exist
                if (!Directory.Exists(inputDirectory))
                {
                    Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
                    return;
                }

                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Get all CF2 files in the input directory
                string[] cf2Files = Directory.GetFiles(inputDirectory, "*.cf2", SearchOption.TopDirectoryOnly);
                if (cf2Files.Length == 0)
                {
                    Console.WriteLine("No CF2 files found to process.");
                    return;
                }

                foreach (string cf2FilePath in cf2Files)
                {
                    // Determine output PDF file path
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(cf2FilePath);
                    string pdfFilePath = Path.Combine(outputDirectory, fileNameWithoutExt + ".pdf");

                    // Load the CF2 file
                    Image cadImage = Image.Load(cf2FilePath);

                    // Configure PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Use a concrete rasterization options class
                    var rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 800,
                        PageHeight = 600
                        // BackgroundColor omitted to avoid System.Drawing usage
                    };

                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Save as PDF
                    cadImage.Save(pdfFilePath, pdfOptions);

                    Console.WriteLine($"Converted '{cf2FilePath}' to PDF successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}