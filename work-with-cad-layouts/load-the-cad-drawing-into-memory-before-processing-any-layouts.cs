using Aspose.CAD.FileFormats.Cad;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image cadImage = Image.Load(inputPath))
            {
                // Configure rasterization options (e.g., select layout and layer)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Example layout and layer names; adjust as needed
                    Layouts = new string[] { "Model" },
                    Layers = new string[] { "0" },
                    PageWidth = 1600,
                    PageHeight = 1200,
                    DrawType = CadDrawTypeMode.UseObjectColor
                };

                // Set up PDF export options
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the CAD drawing as PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Successfully saved PDF to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}