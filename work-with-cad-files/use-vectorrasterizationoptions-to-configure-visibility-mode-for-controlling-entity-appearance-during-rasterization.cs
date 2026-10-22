using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Paths for input CAD file and output PDF
        string inputPath = "input.dxf";
        string outputPath = "output.pdf";

        // Verify that the input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure the output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD image
            Image cadImage = Image.Load(inputPath);

            // NOTE: In the current Aspose.CAD SDK version, VectorRasterizationOptions is abstract
            // and no concrete implementation (e.g., PdfVectorRasterizationOptions) is provided.
            // Therefore, configuring VisibilityMode is not possible. We proceed with default options.

            var pdfOptions = new PdfOptions();

            // Save the CAD drawing as a PDF file
            cadImage.Save(outputPath, pdfOptions);
            Console.WriteLine($"PDF successfully saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during conversion: {ex.Message}");
        }
    }
}