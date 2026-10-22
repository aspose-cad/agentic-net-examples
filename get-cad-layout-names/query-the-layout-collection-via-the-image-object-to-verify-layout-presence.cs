using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.dwg";
            string outputPath = "output.pdf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image cadImage = Image.Load(inputPath))
            {
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                // Example: specify layout names if needed
                // rasterOptions.Layouts = new string[] { "Model" };

                PdfOptions pdfOptions = new PdfOptions();
                pdfOptions.VectorRasterizationOptions = rasterOptions;

                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"File saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}