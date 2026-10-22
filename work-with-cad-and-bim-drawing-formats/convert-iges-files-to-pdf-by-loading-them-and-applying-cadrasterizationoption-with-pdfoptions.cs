using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "input.iges";
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
                // Configure rasterization options for vector output
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();

                // Set up PDF save options and attach rasterization settings
                PdfOptions pdfOptions = new PdfOptions();
                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save the IGES file as PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"Conversion completed: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}