using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image cadImage = Image.Load(inputPath))
            {
                CadRasterizationOptions cadOptions = new CadRasterizationOptions();
                cadOptions.Layouts = new string[] { "Model" };
                // Automatic scaling is default; no additional settings required.

                PdfOptions pdfOptions = new PdfOptions();
                pdfOptions.VectorRasterizationOptions = cadOptions;

                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"File converted successfully: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}