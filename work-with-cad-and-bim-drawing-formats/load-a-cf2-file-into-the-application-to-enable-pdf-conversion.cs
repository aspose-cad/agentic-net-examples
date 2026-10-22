using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadPdfConversion
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.cf2";
            string outputPath = "output.pdf";

            try
            {
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                using (Image cadImage = Image.Load(inputPath))
                {
                    PdfOptions pdfOptions = new PdfOptions();
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"PDF file saved successfully to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}