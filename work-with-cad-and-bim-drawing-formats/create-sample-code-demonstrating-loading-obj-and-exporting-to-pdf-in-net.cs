using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadObjToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputPath = "input.obj";
                string outputPath = "output.pdf";

                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file does not exist: {inputPath}");
                    return;
                }

                using (Image cadImage = Image.Load(inputPath))
                {
                    PdfOptions pdfOptions = new PdfOptions();
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Successfully exported OBJ to PDF: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}