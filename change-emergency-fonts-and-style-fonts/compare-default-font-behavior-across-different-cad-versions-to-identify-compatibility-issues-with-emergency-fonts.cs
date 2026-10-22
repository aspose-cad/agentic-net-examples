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
                // Cast to CadImage if CAD-specific properties are needed
                CadImage cad = cadImage as CadImage;

                // Configure PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                // Save the CAD drawing as PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"CAD file saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}