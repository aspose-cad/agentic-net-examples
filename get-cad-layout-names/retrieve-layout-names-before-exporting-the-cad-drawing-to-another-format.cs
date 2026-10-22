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
            string inputPath = "sample.dwg";
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
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Retrieve layout names
                string[] layoutNames = cadImage.Layouts.Keys.ToArray();
                Console.WriteLine("Available layouts:");
                foreach (string layoutName in layoutNames)
                {
                    Console.WriteLine($"- {layoutName}");
                }

                // Set rasterization options to include the layouts
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layouts = layoutNames // export all layouts
                };

                // Prepare PDF export options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Export to PDF
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"Export completed: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}