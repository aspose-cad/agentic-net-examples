using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadConversionExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder paths – replace with actual file locations.
            string inputPath = "sample.dwf";
            string outputPath = "output.pdf";

            try
            {
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                using (Image dwfImage = Image.Load(inputPath))
                {
                    PdfOptions pdfOptions = new PdfOptions();
                    dwfImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Conversion completed successfully. PDF saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred during conversion: {ex.Message}");
            }
        }
    }
}