using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.dwg";
            string defaultPdfPath = "output_default.pdf";
            string qualityPdfPath = "output_quality.pdf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file once and perform both exports
            using (Image cadImage = Image.Load(inputPath))
            {
                // Export with default options
                PdfOptions defaultPdfOptions = new PdfOptions();
                cadImage.Save(defaultPdfPath, defaultPdfOptions);

                // Export with quality‑adjusted rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Specify the layout(s) to export; adjust as needed
                    Layouts = new string[] { "Model" }
                };

                PdfOptions qualityPdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(qualityPdfPath, qualityPdfOptions);
            }

            // Compare file sizes
            FileInfo defaultInfo = new FileInfo(defaultPdfPath);
            FileInfo qualityInfo = new FileInfo(qualityPdfPath);

            long defaultSize = defaultInfo.Length;
            long qualitySize = qualityInfo.Length;

            Console.WriteLine($"Default PDF size: {defaultSize} bytes");
            Console.WriteLine($"Quality‑adjusted PDF size: {qualitySize} bytes");
            Console.WriteLine($"Size difference: {qualitySize - defaultSize} bytes");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}