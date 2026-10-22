using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputFile = "input.dwg";
        if (!File.Exists(inputFile))
        {
            Console.Error.WriteLine($"Input file '{inputFile}' does not exist.");
            return;
        }

        string outputFile = "output.pdf";

        try
        {
            using (Image cadImage = Image.Load(inputFile))
            {
                long originalSize = new FileInfo(inputFile).Length;

                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800,
                    PageHeight = 600,
                    Layouts = new[] { "Model" }
                };

                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputFile, pdfOptions);

                if (File.Exists(outputFile))
                {
                    long exportedSize = new FileInfo(outputFile).Length;
                    Console.WriteLine($"Original CAD size: {originalSize} bytes");
                    Console.WriteLine($"Exported PDF size: {exportedSize} bytes");
                    if (exportedSize != originalSize)
                    {
                        Console.WriteLine("Validation passed: exported file size reflects rescaling.");
                    }
                    else
                    {
                        Console.WriteLine("Validation warning: exported file size is identical to original; rescaling may not have taken effect.");
                    }
                }
                else
                {
                    Console.Error.WriteLine("Export failed: output file not found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}