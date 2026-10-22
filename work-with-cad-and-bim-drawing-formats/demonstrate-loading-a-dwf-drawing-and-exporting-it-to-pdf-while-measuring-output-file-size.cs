using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadDwfToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input and output paths
                string inputPath = "input.dwf";
                string outputDirectory = "Output";
                string outputPath = Path.Combine(outputDirectory, "output.pdf");

                // Verify input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Ensure output directory exists
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Load the DWF drawing
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Set PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                // Measure output file size
                FileInfo outputInfo = new FileInfo(outputPath);
                long fileSizeBytes = outputInfo.Length;
                Console.WriteLine($"PDF file saved to: {outputPath}");
                Console.WriteLine($"File size: {fileSizeBytes} bytes");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}