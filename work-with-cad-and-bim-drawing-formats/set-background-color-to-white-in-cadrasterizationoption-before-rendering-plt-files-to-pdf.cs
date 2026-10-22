using Aspose.CAD.FileFormats.Cad;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadProcessingDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input PLT file path – replace with an existing PLT file on your system.
                string inputPath = @"C:\Samples\input.plt";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Ensure output directory exists.
                string outputDir = @"C:\Samples";
                Directory.CreateDirectory(outputDir);
                string pdfOutputPath = Path.Combine(outputDir, "output.pdf");

                // Load the CAD image.
                using (Image cadImage = Image.Load(inputPath))
                {
                    Image cad = cadImage ;
                    if (cad == null)
                    {
                        Console.WriteLine("The loaded file is not a supported CAD format.");
                        return;
                    }

                    // Set up PDF export with a white background.
                    PdfOptions pdfOptions = new PdfOptions();
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        BackgroundColor = Color.FromArgb(255, 255, 255) // White background
                    };
                    pdfOptions.VectorRasterizationOptions = rasterOptions;

                    // Save as PDF.
                    cad.Save(pdfOutputPath, pdfOptions);
                    Console.WriteLine($"PDF saved to: {pdfOutputPath}");
                }

                Console.WriteLine("Processing completed.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}