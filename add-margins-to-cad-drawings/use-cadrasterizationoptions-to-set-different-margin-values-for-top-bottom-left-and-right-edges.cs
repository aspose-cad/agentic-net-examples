using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
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

            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the CAD file
            Image cadImage = Image.Load(inputPath);

            // Configure rasterization options.
            // Note: The current Aspose.CAD version does not expose a PageMargins property.
            // If margin control is required, consider adjusting PageSize or using a different rendering approach.
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions();

            // Set up PDF export options
            PdfOptions pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Save the CAD drawing as PDF
            cadImage.Save(outputPath, pdfOptions);
            cadImage.Dispose();

            Console.WriteLine("CAD drawing saved successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
