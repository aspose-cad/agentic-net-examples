using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        string inputPath = "input.cf2";
        string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            var loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            using (Image cadImage = Image.Load(inputPath, loadOptions))
            {
                if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
                {
                    Console.Error.WriteLine("Load completed with errors:");
                    foreach (var error in loadOptions.Errors)
                    {
                        Console.Error.WriteLine(error.Message);
                    }
                }

                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1200,
                    PageHeight = 1200,
                    Layouts = new[] { "Model" }
                };

                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Successfully exported '{inputPath}' to PDF at '{outputPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Exception during conversion: {ex.Message}");
        }
    }
}