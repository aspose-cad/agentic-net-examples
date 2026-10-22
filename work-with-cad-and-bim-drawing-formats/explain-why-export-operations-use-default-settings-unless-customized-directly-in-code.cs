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
            // Define input and output file paths.
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

            // Verify that the input CAD file exists.
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file using Aspose.CAD.Image.Load.
            using (Image cadImage = Image.Load(inputPath))
            {
                // Create PDF export options without setting any properties.
                // Because no properties are customized, the export operation
                // will use the SDK's default settings (page size, resolution,
                // layers, layouts, etc.). This demonstrates that defaults are
                // applied automatically unless the developer explicitly changes them.
                PdfOptions pdfOptions = new PdfOptions();

                // Save the CAD image to PDF using the default options.
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"Export completed successfully: {outputPath}");
        }
        catch (Exception ex)
        {
            // Print concise error information without crashing the application.
            Console.Error.WriteLine($"Error during export: {ex.Message}");
        }
    }
}