using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image cadImage = Image.Load(inputPath))
            {
                CadImage cad = (CadImage)cadImage;

                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layouts = new[] { "Model" },               // replace with the required layout name
                    Layers = new[] { "Layer1", "Layer2" }      // replace with the layers to export
                };

                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cad.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}