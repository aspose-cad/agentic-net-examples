using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

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

            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Loaded file is not a CAD image.");
                    return;
                }

                if (cadImage.Layouts == null || cadImage.Layouts.Count == 0)
                {
                    Console.Error.WriteLine("No layouts found in the CAD drawing.");
                    return;
                }

                // Get the first layout name
                var firstLayout = cadImage.Layouts.First();
                string layoutName = firstLayout.Key;

                // Configure rasterization options for PDF export
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layouts = new[] { layoutName },
                    AutomaticLayoutsScaling = true
                };

                // Configure PDF save options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD drawing as PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"PDF successfully saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}