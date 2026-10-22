using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.Error.WriteLine("Usage: <program> <input.plt> [outputDirectory]");
            return;
        }

        string inputPath = args[0];
        string outputDirectory = args.Length > 1 ? args[1] : Path.Combine(Directory.GetCurrentDirectory(), "Output");

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file does not exist: {inputPath}");
                return;
            }

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            using (Image cadImage = Image.Load(inputPath))
            {
                PdfOptions pdfOptions = new PdfOptions();

                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    LineScale = 1.0f
                };

                pdfOptions.VectorRasterizationOptions = rasterOptions;

                string outputFileName = Path.GetFileNameWithoutExtension(inputPath) + ".pdf";
                string outputPath = Path.Combine(outputDirectory, outputFileName);

                cadImage.Save(outputPath, pdfOptions);

                Console.WriteLine($"Successfully converted '{inputPath}' to PDF at '{outputPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}